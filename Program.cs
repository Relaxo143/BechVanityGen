using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBitcoin;
using System.Threading;

namespace Bitcoin_address_generator
{
	class Program
	{
		public static bool isShuttingDown = false;

		public static Thread t0;
		public static Thread t1;
		public static Thread t2;
		public static Thread t3;
		public static Thread t4;
		public static Thread t5;

		public static Thread lastThread;
		public static int prefixLength;

		static string bech32_CHARS = "023456789acdefghjklmnpqrstuvwxyz"; 

		 static Network net = Network.Main;

		static void Main()
		{

	
			string prefix;

			Console.WriteLine("The following characters are allowed:");
			Console.WriteLine("023456789acdefghjklmnpqrstuvwxyz");
			Console.WriteLine();

			do
			{
				Console.Write("Enter prefix: ");
			    prefix = Console.ReadLine();
			}
			while (!isPrefixValid(prefix));
			prefix = "bc1q" + prefix; // add bech32 v0 identifier
			prefixLength = prefix.Length;


			t0 = new Thread(() => T0_Gen(prefix));
			t1 = new Thread(() => T1_Gen(prefix));
			t2 = new Thread(() => T2_Gen(prefix));
			t3 = new Thread(() => T3_Gen(prefix));
			t4 = new Thread(() => T4_Gen(prefix));
			t5 = new Thread(() => T5_Gen(prefix));


			t0.Start();
			t1.Start();
			t2.Start();
			t3.Start();
			t4.Start();
			t5.Start();
			Console.WriteLine("");
			Console.WriteLine("Looking for an address with the desired pattern, the program will beep once completed. Please wait...");

		}


		static void GenerateAddresses(string prefix)
		{
			int prefixCounter = 0;
			ulong triesCounter = 0;

			string address = "";
			Key key1 = new Key();
			string privkey1;

			while (!isShuttingDown)
			{
				key1 = new Key();
				var secret = new BitcoinSecret(key1, net);
				var addr = secret.GetAddress(ScriptPubKeyType.Segwit);

				address = addr.ToString();

				for (int i = 0; i < prefixLength; i++)
				{
					if (address[i] == prefix[i])
					{
						prefixCounter++;
					}
				}

				if (prefixCounter == prefixLength)
				{

					isShuttingDown = true;
					privkey1 = key1.GetWif(net).ToString();
					Console.Clear();
					lastThread = new Thread(() => ShowDetails(prefix, address, privkey1, triesCounter));
					lastThread.Start();
					StopThreads();
					return;
				}
				
				prefixCounter = 0;
				triesCounter++;
				
			}
		}

		static void T0_Gen(string prefix)
		{

			var process = System.Diagnostics.Process.GetCurrentProcess();
			//process.ProcessorAffinity = new IntPtr(0x0001);

			GenerateAddresses(prefix);

			return;
		}

		static void T1_Gen(string prefix)
		{
			var process = System.Diagnostics.Process.GetCurrentProcess();
			//process.ProcessorAffinity = new IntPtr(0x0002);

			GenerateAddresses(prefix);

			return;
		}

		static void T2_Gen(string prefix)
		{
			var process = System.Diagnostics.Process.GetCurrentProcess();
			//process.ProcessorAffinity = new IntPtr(0x0003);

			GenerateAddresses(prefix);

			return;
		}

		static void T3_Gen(string prefix)
		{
			var process = System.Diagnostics.Process.GetCurrentProcess();
			//process.ProcessorAffinity = new IntPtr(0x0004);

			GenerateAddresses(prefix);

			return;
		}

		static void T4_Gen(string prefix)
		{
			var process = System.Diagnostics.Process.GetCurrentProcess();
			//process.ProcessorAffinity = new IntPtr(0x0005);

			GenerateAddresses(prefix);
			return;
		}

		static void T5_Gen(string prefix)
		{
			var process = System.Diagnostics.Process.GetCurrentProcess();
			//process.ProcessorAffinity = new IntPtr(0x0006);

			GenerateAddresses(prefix);

			return;
		}

		static void StopThreads()
		{
			t0.Abort();
			t1.Abort();
			t2.Abort();
			t3.Abort();
			t4.Abort();
			t5.Abort();
		}

		static bool isPrefixValid(string prefix)
		{
			int validationCounter = 0;

			if (prefix == "" || prefix.Length > 58) return false;

			foreach (char ch in prefix)
			{
				for (int i = 0; i < bech32_CHARS.Length; i++)
				{
					if (ch == bech32_CHARS[i])
					{
						validationCounter++;
					}
				}			
			}

			if (validationCounter == prefix.Length) return true; 
			else return false;

		}

		static void ShowDetails(string prefix, string address, string privkey, ulong tries)
		{
			Console.WriteLine("prefix:" + prefix);
			Console.WriteLine();
			Console.WriteLine("Address: " + address);
			Console.WriteLine("Private Key: " + privkey);
			Console.WriteLine("It took " + tries * 6 + " tries to find this address.");
			Console.Beep(440, 250);
			Console.Beep(650, 250);
			Console.Beep(875, 250);
			Console.WriteLine();
			Console.WriteLine("Job finished. You can close this window when done copying the wallet details.");
			Thread.Sleep(-1);
		}

	}
}

		
	




