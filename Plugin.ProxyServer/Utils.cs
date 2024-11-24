using System;
using System.IO;
using System.Threading.Tasks;

namespace Plugin.ProxyServer
{
	internal static class Utils
	{
		public static T RunTaskSync<T>(Task<T> task)
		{
			try
			{
				if(task.IsCompleted)
					return task.Result;
				else if(task.Wait(3000))
					return task.Result;
				else
					return default;//HACK: На некоторых запросах получение payload'а - зависает. Предположительно, по причине передачи Content-Length>0 и пустого payload'а
			} catch(AggregateException exc)
			{
				if(exc.InnerException != null)
					throw exc.InnerException;
				else throw;
			}
		}

		/// <summary>Преобразовать поток в массив байт</summary>
		/// <returns>Массив байт полученного ответа</returns>
		public static Byte[] ConvertStreamToArray(Stream stream)
		{
			Byte[] buffer = new Byte[4096];
			using(MemoryStream memory = new MemoryStream())
			{
				Int32 count = 0;
				do
				{
					count = stream.Read(buffer, 0, buffer.Length);
					memory.Write(buffer, 0, count);
				} while(count != 0);
				return memory.ToArray();
			}
		}

		/// <summary>Генерация пароля для сертификата</summary>
		/// <param name="length">Длинна пароля</param>
		/// <returns>Результат генерации пароля</returns>
		public static String GeneratePassword(Int32 length = 8)
		{
			const String chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!?@#$%^&*()_=+-[]{}<>\\|/";
			Char[] result = new Char[length];
			Random rnd = new Random();
			for(UInt32 loop = 0; loop < length; loop++)
				result[loop] = chars[rnd.Next(chars.Length)];

			return new String(result);
		}
	}
}