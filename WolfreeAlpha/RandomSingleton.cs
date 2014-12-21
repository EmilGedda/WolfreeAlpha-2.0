using System;

namespace WolfreeAlpha
{
	internal static class RandomSingleton
	{
		private static Random rng;

		public static Random Instance
		{
			get { return rng ?? (rng = new Random()); }
		}
	}
}