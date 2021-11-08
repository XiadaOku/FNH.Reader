using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;




struct HChar {
	const int HFONT_NULL_LEVEL = 128;

	public short SizeX;
	public short SizeY;
	public int Size;
	public int Flags;

	public short RightOffs;
	public short LeftOffs;

	public short MinHeight;
	public short MaxHeight;

	public ushort[] HeightMap;

	public HChar(short sx, short sy, int fl) {
		this.SizeX = sx;
		this.SizeY = sy;
		this.Size = sx * sy;
		this.Flags = fl;

		this.RightOffs = 0;
		this.LeftOffs = 0;
		this.MinHeight = 0;
		this.MaxHeight = 255;
		this.HeightMap = new ushort[this.Size];
	}

	public void calcHLimits() {
		this.MaxHeight = (short)this.HeightMap[0];
		this.MinHeight = (short)this.HeightMap[0];

		for (int i = 1; i < this.Size; i++) {
			if (this.HeightMap[i] < this.MinHeight)
				this.MinHeight = (short)this.HeightMap[i];
			if (this.HeightMap[i] > this.MaxHeight)
				this.MaxHeight = (short)this.HeightMap[i];
		}
	}

	public void calcOffsets() {
		int null_lev = HFONT_NULL_LEVEL;

		RightOffs = (short)(this.SizeX / 2);
		LeftOffs = (short)(this.SizeX / 2);

		for (int i = 0; i < SizeY; i++) {
			int offs = SizeX / 3;
			for (int j = 0; j < SizeX; j++) {
				if (HeightMap[i * SizeX + j] != null_lev) {
					offs = j;
					break;
				}
			}
			if (offs < LeftOffs)
				LeftOffs = (short)offs;

			offs = SizeX / 3;
			for (int j = SizeX - 1; j >= 0; j--) {
				if (HeightMap[i * SizeX + j] != null_lev) {
					offs = SizeX - j - 1;
					break;
				}
			}
			if (offs < RightOffs)
				RightOffs = (short)offs;
		}
	}
};

namespace FNH.Reader {
	public partial class Form1 : Form {
		const string HFntSign = "HFNT 1.01";
		const int NULL_HCHAR = 0x01;
		const int sizeof_HChar = 32;
		const int HFONT_NULL_LEVEL = 128;

		const ushort scale = 256;
		const short lev = 40;
		Color[] gradient = GetGradient(Color.FromArgb(0, 0, 0), Color.FromArgb(166, 255, 0), 32);
		const int black_border = 5;
		const int black_delta = 20;

		int shadow_offs = 5;
		int shadow_color = 2;
		int space = 10;
		bool is_border;
		bool is_shadow;

		Color col = Color.FromArgb(0, 0, 0);
		string str_txt = "";


		string filepath = "";
		
		public Form1() {
		    InitializeComponent();
		}
		
		private void button_openFile_clicked(object sender, EventArgs e) {
		    dialog_openFNH.ShowDialog();
		}
		
		private void OpenFile(object sender, CancelEventArgs e) {
			this.filepath = dialog_openFNH.FileName;

			if (textbox.Text == "") {
				textbox.Text = "a";
			}
			str_txt = textbox.Text;

			try {
				int ex = Convert.ToInt32(spacing.Text);
			}
			catch (FormatException) {
				spacing.Text = "5";
			}
			space = Convert.ToInt32(spacing.Text);

			is_shadow = check_shadow.Checked;
			is_border = check_border.Checked;

			if (Path.GetExtension(this.filepath) == ".fnh") {
				OpenFNH();
			}
			else if (Path.GetExtension(this.filepath) == ".fnt") {
				OpenFNT();
			}
		}


		public static Color[] GetGradient(Color start, Color end, int size) {
			double stepA = ((double)(end.A - start.A) / (size - 1));
			double stepR = ((double)(end.R - start.R) / (size - 1));
			double stepG = ((double)(end.G - start.G) / (size - 1));
			double stepB = ((double)(end.B - start.B) / (size - 1));
			Color[] colors = new Color[size + 1];

			for (int i = 0; i < size; i++) {
				colors[i] = Color.FromArgb(
					start.A + (int)(stepA * i), 
					start.R + (int)(stepR * i),
					start.G + (int)(stepG * i),
					start.B + (int)(stepB * i));
			}
			colors[size] = Color.FromArgb(0, 0, 0, 0);

			return colors;
		}

		private void OpenFNH() {
			Stream bytes = new MemoryStream(File.ReadAllBytes(this.filepath));
			BinaryReader binReader = new BinaryReader(bytes);

			string str = new string(binReader.ReadChars(HFntSign.Length));
			if (str != HFntSign) {
				throw new Exception(String.Format("{0} isn't equal {1}", str, HFntSign));
			}

			//scan_chars
			int memory_size = 0, char_num = 0;
			int dataHeapSize, memHeapSize;

			short SizeX = binReader.ReadInt16();
			short SizeY = binReader.ReadInt16();
			short StartChar = binReader.ReadInt16();
			short EndChar = binReader.ReadInt16();

			int NumChars = EndChar - StartChar + 1;
			for (int Char = 0; Char < NumChars; Char++) {
				short sx = binReader.ReadInt16();
				short sy = binReader.ReadInt16();
				int fl = binReader.ReadInt32();

				if (Char == 0) {
					fl &= ~NULL_HCHAR; //fl % 2 == 0
				}

				if (Char > 0 && !Convert.ToBoolean(fl & NULL_HCHAR)) {
					binReader.Read(new char[sx * sy], 0, sx * sy);
					memory_size += sx * sy;
					char_num++;
				}
				else {
					if (Char == 0) {
						memory_size += sx * sy;
						char_num++;
					}
				}
			}

			dataHeapSize = NumChars * sizeof_HChar;
			memHeapSize = memory_size;


			//load
			binReader.BaseStream.Position = HFntSign.Length;

			SizeX = binReader.ReadInt16();
			SizeY = binReader.ReadInt16();
			StartChar = binReader.ReadInt16();
			EndChar = binReader.ReadInt16();

			NumChars = EndChar - StartChar + 1;
			HChar[] data = new HChar[NumChars];

			for (int Char = 0; Char < NumChars; Char++) {
				short sx = binReader.ReadInt16();
				short sy = binReader.ReadInt16();
				int fl = binReader.ReadInt32();

				if (Char == 0) {
					fl &= ~NULL_HCHAR; //fl % 2 == 0
				}
				data[Char] = new HChar((short)sx, (short)sy, fl);


				for (int pixel = 0; pixel < data[Char].Size; pixel++) {
					data[0].HeightMap[pixel] = HFONT_NULL_LEVEL;
				}

				if (Char > 0 && !Convert.ToBoolean(fl & NULL_HCHAR)) {
					for (int pixel = 0; pixel < data[Char].Size; pixel++) {
						data[Char].HeightMap[pixel] = binReader.ReadByte();
					}
				}

				data[Char].calcHLimits();
				data[Char].calcOffsets();
			}


			//output
			Byte[] bytes_str = Encoding.Unicode.GetBytes(str_txt);
			bytes_str = Encoding.Convert(Encoding.Unicode, Encoding.GetEncoding(866), bytes_str);

			Bitmap img = new Bitmap(box_image.Size.Width / (data[0].SizeX + space) * (data[0].SizeX + space), (int)Math.Ceiling((double)bytes_str.Length / (box_image.Size.Width / (data[0].SizeX + space))) * data[0].SizeY);

			for (int i = 0; i < bytes_str.Length; i++) {
				if (bytes_str[i] <= 32 || bytes_str[i] > 255) {
					continue;
				}

				ushort[] buf = data[bytes_str[i]].HeightMap;

				for (int y = 0; y < data[bytes_str[i]].SizeY; y++) {
					for (int x = 0; x < data[bytes_str[i]].SizeX; x++) {
						buf[y * data[bytes_str[i]].SizeX + x] -= HFONT_NULL_LEVEL;
						buf[y * data[bytes_str[i]].SizeX + x] = (ushort)((buf[y * data[bytes_str[i]].SizeX + x] * scale) >> 8);
						buf[y * data[bytes_str[i]].SizeX + x] += HFONT_NULL_LEVEL;
					}
				}

				for (int y = 0; y < data[bytes_str[i]].SizeY; y++) {
					int num = 0;
					for (int x = 0; x < data[bytes_str[i]].SizeX; x++) {
						int t = buf[y * data[bytes_str[i]].SizeX + x] - HFONT_NULL_LEVEL;
						int m = data[bytes_str[i]].HeightMap[y * data[bytes_str[i]].SizeX + x] - HFONT_NULL_LEVEL;

						if (t != 0 || m != 0) {
							t += lev;
							if (t <= 0) t = 256;
							else if (t > 255) t = 255;

							int act_x = i % (box_image.Size.Width / (data[0].SizeX + space)) * (data[0].SizeX + space) + x + space;
							int act_y = i / (box_image.Size.Width / (data[0].SizeX + space)) * data[0].SizeY + y;

							if (is_border && num < black_border) {
								t = num * black_delta;
							}
							if (is_shadow && act_x != 0 && img.GetPixel(act_x - 1, act_y).A == 0) {
								for (int shadow_x = x - shadow_offs; shadow_x < x; shadow_x++) {
									if (act_x - x + shadow_x < 0) {
										continue;
									}
									img.SetPixel(act_x - x + shadow_x, act_y, gradient[shadow_color]);
								}
							}
							num++;

							img.SetPixel(act_x, act_y, gradient[t / (int)Math.Ceiling((double)256 / gradient.Length)]);
						}
					}
				}
			}

			box_image.Image = img;
			Clipboard.SetImage(img);
		}

		private void OpenFNT() {
			//String че-то там
			Byte[] bytes = File.ReadAllBytes(this.filepath);
			int len = (int)bytes.Length;

			ushort[] LeftOffs = new ushort[256];
			ushort[] RightOffs = new ushort[256];

			int SizeX = 8;
			int SizeY = len / 256;

			for (int s = 0; s < 256; s++) {
				int offs = SizeY * s;
				int align_left = 3;
				int align_right = 3;

				for (int h = 0; h < SizeY; h++) {
					int fl = 0;
					int fr = 0;

					for (int w = 0; w < SizeX; w++) {
						if (fr == 0 && (bytes[offs] & (1 << w)) != 0 && w < align_right) {
							align_right = w;
							fr = 1;
						}
						if (fl == 0 && (bytes[offs] & (1 << (7 - w))) != 0 && w < align_left) {
							align_left = w;
							fl = 1;
						}
					}
					offs++;
				}
				if (s != (int)' ') {
					LeftOffs[s] = (ushort)align_left;
					RightOffs[s] = (ushort)align_right;
				}
				else {
					LeftOffs[s] = 0;
					RightOffs[s] = 0;
				}
			}


			//output
			Byte[] str = Encoding.Unicode.GetBytes(str_txt);
			str = Encoding.Convert(Encoding.Unicode, Encoding.GetEncoding(866), str);

			Bitmap img = new Bitmap(box_image.Size.Width / (SizeX + space) * (SizeX + space), (int)Math.Ceiling((double)box_image.Size.Height / (box_image.Size.Width / (SizeX + space))) * (SizeY + space));

			int X = 0;

			for (int i = 0; i < str.Length; i++) {
				int offs = str[i] * SizeY;

				for (int y = 0; y < SizeY; y++) {
					for (int x = 0; x < SizeX; x++) {
						if ((bytes[offs] & (1 << (7 - x))) != 0) {
							img.SetPixel((x + X) % img.Size.Width, i / (img.Size.Width / (SizeX + space)) * (SizeY + space) + y, col);
						}
					}
					offs++;
				}
				X += SizeX + space;
			}

			box_image.Image = img;
		}


		private void button_savePNG_clicked(object sender, EventArgs e) {
			dialog_savePNG.ShowDialog();
		}

		private void savePNG(object sender, CancelEventArgs e) {
			box_image.Image.Save(dialog_savePNG.FileName);
		}

		private void text_TextChanged(object sender, EventArgs e) {
			if (this.filepath == "") {
				return;
			}

			if (textbox.Text == "") {
				textbox.Text = "a";
			}
			str_txt = textbox.Text;

			if (Path.GetExtension(this.filepath) == ".fnh") {
				OpenFNH();
			}
			else if (Path.GetExtension(this.filepath) == ".fnt") {
				OpenFNT();
			}
		}

		private void spacing_TextChanged(object sender, EventArgs e) {
			if (this.filepath == "") {
				return;
			}

			try {
				int ex = Convert.ToInt32(spacing.Text);
			}
			catch (FormatException) {
				spacing.Text = "5";
			}
			space = Convert.ToInt32(spacing.Text);

			if (Path.GetExtension(this.filepath) == ".fnh") {
				OpenFNH();
			}
			else if (Path.GetExtension(this.filepath) == ".fnt") {
				OpenFNT();
			}
		}

		private void check_shadow_CheckedChanged(object sender, EventArgs e) {
			if (this.filepath == "") {
				return;
			}

			is_shadow = check_shadow.Checked;

			if (Path.GetExtension(this.filepath) == ".fnh") {
				OpenFNH();
			}
			else if (Path.GetExtension(this.filepath) == ".fnt") {
				OpenFNT();
			}
		}

		private void check_border_CheckedChanged(object sender, EventArgs e) {
			if (this.filepath == "") {
				return;
			}

			is_border = check_border.Checked;

			if (Path.GetExtension(this.filepath) == ".fnh") {
				OpenFNH();
			}
			else if (Path.GetExtension(this.filepath) == ".fnt") {
				OpenFNT();
			}
		}
	}
}
