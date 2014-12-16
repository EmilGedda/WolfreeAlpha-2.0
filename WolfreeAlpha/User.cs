using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WolfreeAlpha
{
	class User
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }

		public string FullName
		{
			get { return String.Concat(FirstName, " ", LastName); }
		}

		public User()
		{
			
		}

		public void GenerateFirstName()
		{
			
		}
		public void GenerateLastName()
		{

		}
	}
}
