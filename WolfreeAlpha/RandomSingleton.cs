using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WolfreeAlpha
{
	static class RandomSingleton
	{
		private static Random rng;
		public static Random Instance{ get { return rng ?? (rng = new Random()); } }
	}
}
