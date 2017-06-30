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

		public override int GetHashCode()
		{
			
		}
		/*
		 * public static byte[] GetHash(string inputString)
{
    HashAlgorithm algorithm = MD5.Create();  //or use SHA256.Create();
    return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
}

public static string GetHashString(string inputString)
{
    StringBuilder sb = new StringBuilder();
    foreach (byte b in GetHash(inputString))
        sb.Append(b.ToString("X2"));

    return sb.ToString();
}
		 */
	}
}