using ProjectPracticeWeb.AppCode;

namespace ProjectPracticeWeb.Models
{
	public class Beverage
	{
		public string Name { get; set; }
		public int Cost { get; set; }
		public int Count { get; set; }

		public override bool Equals(object obj)
		{
			if (obj == null || obj.GetType() != typeof(Beverage))
			{
				return false;
			}

			var bev = (Beverage)obj;

			return bev.Cost == Cost && bev.Count == Count && bev.Name == Name;
		}

		public override string ToString()
		{
			return $"{Name}{Cost}{Count}";
		}

		public override int GetHashCode()
		{
			var stringPresentation = Name + Cost + Count;
			var hashString = CommonMethods.GetHashString(stringPresentation);
			return int.Parse(hashString);
		}		

	}
}