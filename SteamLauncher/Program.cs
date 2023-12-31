﻿using System.Diagnostics;
using System.Drawing;
using SteamLauncher;

string PSYNC_EXE_FILE = "AI_TheSomniumFiles.exe";

string ubicacion = Directory.GetParent(AppContext.BaseDirectory)!.ToString();
string exe_path = Path.Combine(ubicacion, PSYNC_EXE_FILE);
if (!File.Exists(exe_path))
    throw new FileNotFoundException($"No se ha encontrado {PSYNC_EXE_FILE}");
    

OptionValue option = new OptionValue(ubicacion);
option.Load();
option.Value.msaa = OptionValue.MSAA.X16;
option.Value.quality = OptionValue.Quality.FANTASTIC;
option.Value.window = OptionValue.Window.FULLSCREEN;
option.Value.bVSync = true;
option.Value.hint = 1;
option.Value.bDebug = false;
option.Value.bShowLauncher = false;
option.Value.size = new Size(1280, 720); // Lo mismo funciona bien con la resolución normal.
option.Save();

Process proceso = new Process();
proceso.StartInfo.FileName = exe_path;
proceso.StartInfo.Arguments = option.GetArg();
proceso.Start();