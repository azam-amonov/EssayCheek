// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, World!");
var num = "1000";
var toPrint = Convert.ToInt32(num);

 int ToDecimal(string binaryString)
 {
	 var binaryNumber = Convert.ToInt32(binaryString);
	 int decimalValue = 0;
	 int base1 = 1;
	 while (binaryNumber > 0)
	 {
		 int reminder = binaryNumber % 10;
		 binaryNumber /= 10;
		 decimalValue += reminder * base1;
		 base1 *= 2;
	 }
	 return decimalValue;
 }

string ToBinary(int decimalValue)
{
	int[] binary = new int[10];
	var result = "";
	int i;
	
	for (i = 0; decimalValue > 0; i++)
	{
		binary[i] = decimalValue % 2;
		decimalValue /= 2;
	}

	for (i -= 1; i >= 0; i--)
		result += Convert.ToString(binary[i]);
	
	return result;
}

