﻿using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;

namespace SweetFX_Configurator
{
    public static class Settings
    {
        private static IniFile ini;

        public static void Load()
        {
            string path = Application.StartupPath + "\\settings.ini";
            if (!File.Exists(path)) { File.Create(path).Close(); }
            //
            ini = new IniFile(path);
        }

        #region Main_Window_Geometry

        public static string Main_Window_Geometry
        {
            get
            {
                return ini.GetString("Settings", "Main_Window_Geometry", "");
            }
            set
            {
                ini.WriteValue("Settings", "Main_Window_Geometry", value);
            }
        }

        #endregion

        #region GameManager_Window_Geometry

        private static string gmwg = null;

        public static string GameManager_Window_Geometry
        {
            get
            {
                if (gmwg == null)
                {
                    gmwg = ini.GetString("Settings", "GameManager_Window_Geometry", "");
                }
                return gmwg;
            }
            set
            {
                ini.WriteValue("Settings", "GameManager_Window_Geometry", value);
                gmwg = value;
            }
        }

        #endregion

        #region AddGame_Window_Geometry

        private static string awg = null;

        public static string AddGame_Window_Geometry
        {
            get
            {
                if (awg == null)
                {
                    awg = ini.GetString("Settings", "AddGame_Window_Geometry", "");
                }
                return awg;
            }
            set
            {
                ini.WriteValue("Settings", "AddGame_Window_Geometry", value);
                awg = value;
            }
        }

        #endregion

        #region Settings_Window_Geometry

        private static string swg = null;

        public static string Settings_Window_Geometry
        {
            get
            {
                if (swg == null)
                {
                    swg = ini.GetString("Settings", "Settings_Window_Geometry", "");
                }
                return swg;
            }
            set
            {
                ini.WriteValue("Settings", "Settings_Window_Geometry", value);
                swg = value;
            }
        }

        #endregion

        #region About_Window_Geometry

        private static string abwg = null;

        public static string About_Window_Geometry
        {
            get
            {
                if (abwg == null)
                {
                    abwg = ini.GetString("Settings", "About_Window_Geometry", "");
                }
                return abwg;
            }
            set
            {
                ini.WriteValue("Settings", "About_Window_Geometry", value);
                abwg = value;
            }
        }

        #endregion

        #region InstallManager_Window_Geometry

        private static string imwg = null;

        public static string InstallManager_Window_Geometry
        {
            get
            {
                if (imwg == null)
                {
                    imwg = ini.GetString("Settings", "InstallManager_Window_Geometry", "");
                }
                return imwg;
            }
            set
            {
                ini.WriteValue("Settings", "InstallManager_Window_Geometry", value);
                imwg = value;
            }
        }

        #endregion

        #region AddInstall_Window_Geometry

        private static string aiwg = null;

        public static string AddInstall_Window_Geometry
        {
            get
            {
                if (aiwg == null)
                {
                    aiwg = ini.GetString("Settings", "AddInstall_Window_Geometry", "");
                }
                return aiwg;
            }
            set
            {
                ini.WriteValue("Settings", "AddInstall_Window_Geometry", value);
                aiwg = value;
            }
        }

        #endregion

        #region LastGame

        private static Game _lg = null;

        public static Game LastGame
        {
            get
            {
                if (_lg == null)
                {
                    string section = ini.GetString("Settings", "Last_Game", "");
                    if (String.IsNullOrEmpty(section)) { return null; }
                    section = gameNameSaferizer(section);
                    bool exists = false;
                    foreach (string sec in ini.GetSectionNames()) { if (sec == section) { exists = true; } }
                    if (exists)
                        _lg = new Game(ini.GetString(section, "Name", ""), new DirectoryInfo(ini.GetString(section, "Directory", "")));
                    else { return null; }
                }
                return _lg;
            }
            set
            {
                ini.WriteValue("Settings", "Last_Game", value.Name);
                _lg = value;
            }
        }

        #endregion

        public static bool OnlyActive
        {
            get
            {
                return Convert.ToBoolean(ini.GetInt32("Settings", "Only_Active", 0));
            }
            set { ini.WriteValue("Settings", "Only_Active", Convert.ToInt32(value)); }
        }

        public static int LastTab
        {
            get { return ini.GetInt32("Settings", "Last_Tab", 0); }
            set { ini.WriteValue("Settings", "Last_Tab", value); }
        }

        public static Game GetGame(string game)
        {
            string section = gameNameSaferizer(game);
            return new Game(ini.GetString(section, "Name", ""), new DirectoryInfo(ini.GetString(section, "Directory", "")));
        }

        public static List<Game> GetGames()
        {
            string[] game_keys = ini.GetSectionNames();
            List<Game> games = new List<Game>();
            foreach (string key in game_keys)
            {
                if (key != "Settings") { games.Add(new Game(ini.GetString(key, "Name", ""), new DirectoryInfo(ini.GetString(key, "Directory", "")))); }
            }
            return games;
        }

        public static void AddGame(Game _game)
        {
            string section = gameNameSaferizer(_game.Name);
            ini.WriteValue(section, "Name", _game.Name);
            ini.WriteValue(section, "Directory", _game.Directory.FullName);;
        }

        public static void EditGame(Game old_game, Game new_game)
        {
            string section = gameNameSaferizer(old_game.Name);
            ini.DeleteSection(section);
            section = gameNameSaferizer(new_game.Name);
            ini.WriteValue(section, "Name", new_game.Name);
            ini.WriteValue(section, "Directory", new_game.Directory.FullName);
        }

        public static void DeleteGame(Game _game)
        {
            ini.DeleteSection(gameNameSaferizer(_game.Name));
        }

        public static string gameNameSaferizer(string game_name)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                game_name = game_name.Replace(c.ToString(), "");
            }
            return game_name.ToLower().Replace(" ", "_");
        }
    }
}
