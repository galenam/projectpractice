using ProjectPracticeWeb.AppCode;

namespace ProjectPracticeWeb.Models
{
    public class Coin
    {
        public int Nominal { get; set; }
        public int Count { get; set; }

		public override bool Equals(object obj)
		{
			if (obj == null || obj.GetType() != typeof(Coin))
			{
				return false;
			}

			var coin = (Coin)obj;

			return coin.Count == Count && coin.Nominal == Nominal;
		}

		public override int GetHashCode()
		{
			var stringPresentation = (Nominal + Count).ToString();
			var hashString = CommonMethods.GetHashString(stringPresentation);
			return int.Parse(hashString);
		}
	}
}