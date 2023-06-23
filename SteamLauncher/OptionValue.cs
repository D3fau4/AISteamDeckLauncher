using System;
using System.Drawing;
using System.IO;

namespace SteamLauncher;

public class OptionValue
{
	[Flags]
	public enum Quality
	{
		LOW = 0,
		NORMAL = 1,
		FANTASTIC = 2
	}

	public enum Window
	{
		BORDERLESS,
		WINDOW,
		FULLSCREEN
	}

	public enum MSAA
	{
		NONE = 0,
		X2 = 2,
		X4 = 4,
		X8 = 8,
		X16 = 0x10
	}

	public struct SettingValue
	{
		public Quality quality;

		public Window window;

		public MSAA msaa;

		public Size size;

		public bool bVSync;

		public bool bFiltering;

		public int hint;

		public bool bDebug;

		public bool bShowLauncher;
	}

	public const string DEFAULT_FILE_NAME = "launcher.ini";

	public const string MYDOCUMENT_SUBDIRECTORY = "\\SpikeChunsoft\\AI The Somnium Files";

	public const string KW_LAUNCHER = "skip=";

	public const string KW_QUALITY = "quality=";

	public const string KW_WINDOW = "window=";

	public const string KW_MSAA = "msaa=";

	public const string KW_WIDTH = "width=";

	public const string KW_HEIGHT = "height=";

	public const string KW_VSYNC = "vsync=";

	public const string KW_FILTERING = "filtering=";

	public const string KW_HINT = "hint=";

	public const string KW_DEBUG = "debug=";

	public SettingValue Value;

	private string m_FileName;

	private string m_FilePath;

	public OptionValue(string f)
	{
		Value = default(SettingValue);
		m_FileName = "launcher.ini";
		m_FilePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\SpikeChunsoft\\AI The Somnium Files\\";
		if (!Directory.Exists(m_FilePath))
		{
			try
			{
				Directory.CreateDirectory(m_FilePath);
			}
			catch (Exception ex)
			{
				m_FilePath = f + "\\";
				Console.Write("Error:" + ex.ToString());
			}
		}
		SetDefaultValue();
	}

	public bool Load(string file)
	{
		if (File.Exists(file))
		{
			try
			{
				StreamReader streamReader = new StreamReader(file);
				while (!streamReader.EndOfStream)
				{
					string? text = streamReader.ReadLine();
					if (text.IndexOf("skip=") >= 0)
					{
						text = text.Replace("skip=", "");
						Value.bShowLauncher = Convert.ToBoolean(text);
					}
					else if (text.IndexOf("quality=") >= 0)
					{
						text = text.Replace("quality=", "");
						Value.quality = (Quality)Convert.ToInt32(text);
					}
					else if (text.IndexOf("window=") >= 0)
					{
						text = text.Replace("window=", "");
						Value.window = (Window)Convert.ToInt32(text);
					}
					else if (text.IndexOf("msaa=") >= 0)
					{
						text = text.Replace("msaa=", "");
						Value.msaa = (MSAA)Convert.ToInt32(text);
					}
					else if (text.IndexOf("width=") >= 0)
					{
						text = text.Replace("width=", "");
						Value.size.Width = Convert.ToInt32(text);
					}
					else if (text.IndexOf("height=") >= 0)
					{
						text = text.Replace("height=", "");
						Value.size.Height = Convert.ToInt32(text);
					}
					else if (text.IndexOf("vsync=") >= 0)
					{
						text = text.Replace("vsync=", "");
						Value.bVSync = Convert.ToBoolean(text);
					}
					else if (text.IndexOf("filtering=") >= 0)
					{
						text = text.Replace("filtering=", "");
						Value.bFiltering = Convert.ToBoolean(text);
					}
					else if (text.IndexOf("hint=") >= 0)
					{
						text = text.Replace("hint=", "");
						Value.hint = Convert.ToInt32(text);
					}
				}
				streamReader.Close();
				return true;
			}
			catch (Exception ex)
			{
				Console.Write("Error:" + ex.ToString());
			}
		}
		return false;
	}

	public bool Save(string file)
	{
		try
		{
			StreamWriter streamWriter = new StreamWriter(file);
			streamWriter.WriteLine("[RenderOption]");
			streamWriter.WriteLine("quality=" + Convert.ToInt32(Value.quality));
			streamWriter.WriteLine("window=" + Convert.ToInt32(Value.window));
			streamWriter.WriteLine("msaa=" + Convert.ToInt32(Value.msaa));
			streamWriter.WriteLine("width=" + Value.size.Width);
			streamWriter.WriteLine("height=" + Value.size.Height);
			streamWriter.WriteLine("vsync=" + Value.bVSync);
			streamWriter.WriteLine("filtering=" + Value.bFiltering);
			streamWriter.WriteLine("hint=" + Value.hint);
			streamWriter.Close();
			return true;
		}
		catch (Exception ex)
		{
			Console.Write("Error:" + ex.ToString());
		}
		return false;
	}

	public bool Save()
	{
		return Save(m_FilePath + m_FileName);
	}

	public bool Load()
	{
		return Load(m_FilePath + m_FileName);
	}

	public void SetDefaultValue()
	{
		Value.quality = Quality.NORMAL;
		Value.window = Window.WINDOW;
		Value.msaa = MSAA.NONE;
		Value.size = new Size(960, 544);
		Value.bVSync = false;
		Value.bFiltering = false;
		Value.bShowLauncher = false;
		Value.hint = 0;
		Value.bDebug = false;
	}

	public string GetArg()
	{
		string text = "";
		text += ((Value.window == Window.BORDERLESS) ? "-popupwindow" : "");
		text = text + " quality=" + Convert.ToInt32(Value.quality);
		text = text + " window=" + ((Value.window == Window.FULLSCREEN) ? "0" : ((Value.window == Window.WINDOW) ? "1" : "2"));
		text = text + " msaa=" + Convert.ToInt32(Value.msaa);
		text = text + " width=" + Value.size.Width;
		text = text + " height=" + Value.size.Height;
		text = text + " vsync=" + Convert.ToInt32(Value.bVSync);
		text = text + " filtering=" + Convert.ToInt32(Value.bFiltering);
		text = text + " hint=" + Value.hint;
		if (Value.bDebug)
		{
			text += " debug=1";
		}
		return text;
	}
}
