namespace SisandAirlines.Shared.Validators
{
    public static class BrazilianDocumentValidator
    {
        public static bool IsValidCpf(string cpf)
        {
            int[] multiplierOne = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplierTwo = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string baseCpf;
            string checkDigits;
            int sum;
            int remainder;

            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");

            if (cpf.Length != 11)
                return false;

            baseCpf = cpf.Substring(0, 9);
            sum = 0;

            for (int i = 0; i < 9; i++)
                sum += int.Parse(baseCpf[i].ToString()) * multiplierOne[i];

            remainder = sum % 11;
            if (remainder < 2)
                remainder = 0;
            else
                remainder = 11 - remainder;

            checkDigits = remainder.ToString();
            baseCpf += checkDigits;

            sum = 0;
            for (int i = 0; i < 10; i++)
                sum += int.Parse(baseCpf[i].ToString()) * multiplierTwo[i];

            remainder = sum % 11;
            if (remainder < 2)
                remainder = 0;
            else
                remainder = 11 - remainder;

            checkDigits += remainder.ToString();

            return cpf.EndsWith(checkDigits);
        }
    }
}
