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

namespace ChromePassDecrypter
{
	/// <summary>
	/// Contains website, username and password extracted from Google Chrome's password file.
	/// </summary>
	public class Credential
	{
		string password;
		string url;
		string username;
		
		/// <summary>
		/// Returns the saved password.
		/// </summary>
		public string Password
		{
			get
			{
				return this.password;
			}
		}
		
		/// <summary>
		/// Returns the saved URL.
		/// </summary>
		public string Url
		{
			get
			{
				return this.url;
			}
		}
		
		/// <summary>
		/// Returns the saved username.
		/// </summary>
		public string Username
		{
			get
			{
				return this.username;
			}
		}
		
		/// <summary>
		/// Creates a new instance of the Credential class with the given url, username and password.
		/// </summary>
		internal Credential(string url, string username, string password)
		{
			this.password = password;
			this.username = username;
			this.url = url;
		}
		
		/// <summary>
		/// Returns all values each seperated with a comma.
		/// </summary>
		/// <returns>Something like url,username,password will be returned.</returns>
		public override string ToString()
		{
			return string.Format("{0},{1},{2}", url, username, password);
		}
		
		/// <summary>
		/// Returns all values with a given format.
		/// </summary>
		/// <param name="format"></param>
		/// <returns></returns>
		public string ToString(string format)
		{
			return string.Format(format, url, username, password);
		}
 
	}
}
