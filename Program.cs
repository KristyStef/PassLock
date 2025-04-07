/*
komentáře kódu
práce se soubory nebo externími dat. zdroji (csv, xml....) 
bezchybné načítání dat
bezchybné ukládání dat
min 3. ošetřené vstupy od uživatele
menu – volby – funkce
části generované umělou inteligencí je nutno v dokumentaci označit
*/

Random rnd = new Random();

void Menu()
{
    Console.WriteLine("Welcome to PassLock\n");
    Console.WriteLine("1. Generate password\n2. Save password\n3. Exit\n");
    Console.Write("Enter your choice: ");
    var choice = int.Parse(Console.ReadLine());
    switch (choice)
    {
        case 1:
            Console.Clear();
            Console.WriteLine("Your password should be at least 12 characters long.");
            Console.WriteLine("Count of lowercase letters (3 recommended): ");
            var lowers = int.Parse(Console.ReadLine());
            Console.WriteLine("Count of uppercase letters (4 recommended): ");
            var uppers = int.Parse(Console.ReadLine());
            Console.WriteLine("Count of numbers (3 recommended): ");
            var numbers = int.Parse(Console.ReadLine());
            Console.WriteLine("Count of special symbols (2 recommended): ");
            var symbols = int.Parse(Console.ReadLine());
            Generate(lowers, uppers, numbers, symbols);
            break;
        case 2:
            break;
        case 3:
            break;
    }
}

string Generate(int l, int u, int n, int s)
{
    string password = "";
    int total = l+u+n+s;
    string[] lowerCase = {
        "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z"
    };
    string[] upperCase = {
        "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"
    };
    string[] numbers = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
    string[] specialCharacters = {
        "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "-", "_", "=", "+",
        "[", "]", "{", "}", ";", ":", ",", "<", ".", ">", "/", "?"
    };
    for (int i = 0; i < total; i++)
    {
        switch (rnd.Next(1, 4))
        {
            case 1:
                password += lowerCase[rnd.Next(0, lowerCase.Length)];
                        break;
            case 2:
                password += upperCase[rnd.Next(0, upperCase.Length)];
                break;
            case 3:
                password += numbers[rnd.Next(0, numbers.Length)];
                break;
            case 4:
                password += specialCharacters[rnd.Next(0, specialCharacters.Length)];
                break;
        }
    }
    return password;
};

Menu();