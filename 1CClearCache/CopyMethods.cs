using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace _1CClearCache
{
    public class CopyMethods
    {
        public List<string> UsersFolders { get; set; }

        public CopyMethods()
        {
            UsersFolders = new List<string>();
            GetFolders();
        }

        private void GetFolders()
        {
            List<string> listfolders = new List<string>();
            try
            {

                string[] notUse = new string[] { "All Users", "Default User", "Public", "Администратор", "Все пользователи" };
                foreach (var folder in System.IO.Directory.GetDirectories(@"c:\users"))
                {
                    if (!notUse.Contains(folder.Replace(@"c:\users\", "")))
                    {
                        listfolders.Add(folder);
                    }
                }

                UsersFolders = listfolders;
            }
            catch (Exception)
            {
            }
        }

        public void Delete1CCache()
        {
            ClearCache(UsersFolders, @"AppData\Roaming\1C\1cv8");
            ClearCache(UsersFolders, @"AppData\Roaming\1C\Файлы");
            ClearCache(UsersFolders, @"AppData\Local\1C\1cv8");
        }

        public void ClearCache(List<string> arraySourceFolders, string destinationPath)
        {
            foreach (string item in arraySourceFolders)
            {
                try
                {
                    var path = System.IO.Path.Combine(item, destinationPath);
                    System.IO.Directory.Delete(path, true);
                }
                catch (Exception)
                {
                }
            }
        }

        /// <summary>
        /// Метод завершения процесса через PSExec
        /// </summary>
        /// <param name="host">имя или ip адрес хоста</param>
        /// <param name="login">логин пользователя хоста</param>
        /// <param name="password">пароль пользователя хоста</param>
        /// <param name="nameProcess">имя завершаемого процесса</param>
        public void KillRemoteProcess(string nameProcess)
        {
            string quere = $"/c TaskKill /F /IM {nameProcess}";
            ExecuteProgram("cmd.exe", quere, true);
        }


        /// <summary>
        /// Метод запуска программы
        /// </summary>
        /// <param name="fileName">Имя программы, которая будет запускаться</param>
        /// <param name="arguments">Аргументы программы</param>
        /// <param name="wait">Параметр ожидания завершения программы</param>
        /// <param name="visible">Параметр видимости программы</param>
        /// <param name="directory">Путь к программе</param>
        /// <param name="admin">Параметр запуска программы под правами администратора</param>
        /// <param name="username">Пользователь, под которым будет запускаться программа</param>
        /// <param name="password">Пароль пользователя, при запуске с правами админа</param>
        /// <returns>Код завершения программы, при корректном завершении программы = 0</returns>
        public static int ExecuteProgram(string fileName, string arguments = "", bool wait = false, bool visible = true, string directory = "", bool admin = false, string username = "", string password = "")
        {
            Process process = null;
            int exitCode = 0;
            try
            {
                process = new Process
                {
                    StartInfo = { FileName = fileName }
                };
                if (directory != string.Empty)
                {
                    process.StartInfo.WorkingDirectory = directory;
                }
                if (!string.IsNullOrWhiteSpace(arguments))
                {
                    process.StartInfo.Arguments = arguments;
                }
                if (!visible)
                {
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                }
                if (admin)
                {
                    process.StartInfo.UseShellExecute = true;
                    process.StartInfo.Verb = "runas";
                }
                if ((!admin && !string.IsNullOrWhiteSpace(username)) && !string.IsNullOrWhiteSpace(password))
                {
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.UserName = username;
                    process.StartInfo.Password = ReadPassword(password);
                    process.StartInfo.LoadUserProfile = true;
                }
                process.Start();
                if (wait)
                {
                    process.WaitForExit();
                    exitCode = process.ExitCode;
                }
            }
            catch (Exception)
            {
                exitCode = 1;
            }
            finally
            {
                if (process != null)
                {
                    process.Close();
                }
            }
            return exitCode;
        }

        /// <summary>
        /// Метод безопасного чтения пароля, применяется для передачи пароля приложению
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static SecureString ReadPassword(string password)
        {
            SecureString str = new SecureString();
            for (int i = 0; i < password.Length; i++)
            {
                str.AppendChar(password[i]);
            }
            return str;
        }


    }
}
