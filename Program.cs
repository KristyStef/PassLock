using System.Text;
using System.Xml;

//Initialize all needed variables.
Random rnd = new Random();
bool run = true;
string[] lowerCase =
{
    "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w",
    "x", "y", "z"
};
string[] upperCase =
{
    "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W",
    "X", "Y", "Z"
};
string[] numbers = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
string[] specialCharacters =
{
    "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "-", "_", "=", "+",
    "[", "]", "{", "}", ";", ":", ",", "<", ".", ">", "/", "?"
};

//Function to foolproof all kinds of integer inputs
int ReadInt(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        try
        {
            return int.Parse(Console.ReadLine());
        }
        catch (FormatException)
        {
            Console.WriteLine("Invalid input. Please enter a number.");
        }
        catch (OverflowException)
        {
            Console.WriteLine("Number too large or too small.");
        }
    }
}

//Main function with UI and function calls
void Menu()
{
    Console.WriteLine("Welcome to PassLock\n");
    Console.WriteLine("1. Generate password\n2. Verify password\n3. Exit\n");
    int choice = ReadInt("Please enter your choice: ");

    switch (choice)
    {
        case 1: //Generate password
            Console.Clear();
            Console.WriteLine("Your password should be at least 12 characters long.");
            int lowers = ReadInt("Count of lowercase letters (3 recommended): ");
            int uppers = ReadInt("Count of uppercase letters (4 recommended): ");
            int numbers = ReadInt("Count of numbers (3 recommended): ");
            int symbols = ReadInt("Count of special symbols (2 recommended): ");
            var pass = Generate(lowers, uppers, numbers, symbols);
            Console.Clear();
            Console.WriteLine("Save password to:\n1. CSV\n2. XML");
            choice = ReadInt("Please enter your choice: ");
            SavePass(choice, pass);
            break;
        case 2: //Verify password
            Console.WriteLine("Enter your password: ");
            var passVer = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(passVer))
            {
                Console.WriteLine("Password cannot be empty.");
                return;
            }
            VerifyPass(passVer);
            break;
        case 3: //Exit
            run = false;
            break;
        default:
            Console.Clear();
            Console.WriteLine("Invalid choice.");
            break;
    }
}

//Function to generate a password with parameters given in Menu()
string Generate(int l, int u, int n, int s)
{
    string password = "";

    for (int j = 0; j < l; j++)
    {
        password += lowerCase[rnd.Next(0, lowerCase.Length)];
    }

    for (int j = 0; j < u; j++)
    {
        password += upperCase[rnd.Next(0, upperCase.Length)];
    }

    for (int j = 0; j < n; j++)
    {
        password += numbers[rnd.Next(0, numbers.Length)];
    }

    for (int j = 0; j < s; j++)
    {
        password += specialCharacters[rnd.Next(0, specialCharacters.Length)];
    }

    char[] chars = password.ToCharArray();
    for (int i = chars.Length - 1; i > 0; i--)
    {
        int j = rnd.Next(i + 1);
        (chars[i], chars[j]) = (chars[j], chars[i]);
    }

    password = string.Join("", chars);
    return password;
}

//Function called in Menu() after generating a password. CSV and XML formats
void SavePass(int fileChoice, string pass)
{
    //Saves to desktop for easy access
    string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

    while (true)
    {
        switch (fileChoice)
        {
            case 1: // CSV
                string csvPath = Path.Combine(desktopPath, "passOutput.csv");
                string csvData = $"{pass},{DateTime.Now}";

                if (File.Exists(csvPath))
                {
                    File.AppendAllText(csvPath, "\n" + csvData);
                }
                else
                {
                    File.WriteAllText(csvPath, "Password,Date\n" + csvData);
                }

                Console.Clear();
                Console.WriteLine("The password has been saved to your desktop in 'passOutput.csv'");
                return;

            case 2: // XML
                string xmlPath = Path.Combine(desktopPath, "passOutput.xml");

                if (File.Exists(xmlPath))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(xmlPath);

                    XmlNode newPasswordNode = doc.CreateElement("password");

                    XmlNode rawNode = doc.CreateElement("raw");
                    rawNode.InnerText = pass;
                    newPasswordNode.AppendChild(rawNode);

                    XmlNode dateNode = doc.CreateElement("date");
                    dateNode.InnerText = DateTime.Now.ToString();
                    newPasswordNode.AppendChild(dateNode);

                    doc.DocumentElement.AppendChild(newPasswordNode);
                    doc.Save(xmlPath);
                }
                else
                {
                    string newXml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n<passwords>\n" +
                                    $"<password>\n<raw>{pass}</raw>\n<date>{DateTime.Now}</date>\n</password>\n</passwords>";
                    File.WriteAllText(xmlPath, newXml, Encoding.UTF8);
                }

                Console.Clear();
                Console.WriteLine("The password has been saved to your desktop in 'passOutput.xml'");
                return;

            default:
                Console.Clear();
                Console.WriteLine("Invalid input.\nSave password to:\n1. CSV\n2. XML");
                Console.Write("Please enter your choice: ");

                try
                {
                    fileChoice = int.Parse(Console.ReadLine());
                }
                catch (FormatException)
                {
                    Console.WriteLine("That was not a number. Try again.");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Unexpected error: {e.Message}");
                }
                break;
        }
    }
}

//Function to verify a password's strength via length and characters (Amount of each) used
void VerifyPass(string passVer)
{
    int lCount = 0;
    int uCount = 0;
    int nCount = 0;
    int sCount = 0;

    Console.Clear();
    char[] tempPass = passVer.ToCharArray();
    if (Enumerable.Range(8, 11).Contains(tempPass.Length))
    {
        Console.WriteLine("Password is moderately short.\n");
    }
    else if (Enumerable.Range(0, 7).Contains(tempPass.Length))
    {
        Console.WriteLine("Password is too short.\n");
    }
    else if (tempPass.Length >= 12)
    {
        Console.WriteLine("Password is long enough.\n");
    }

    for (int i = 0; i < tempPass.Length; i++)
    {
        if (lowerCase.Contains(Convert.ToString(tempPass[i])))
        {
            lCount += 1;
        }
        else if (upperCase.Contains(Convert.ToString(tempPass[i])))
        {
            uCount += 1;
        }
        else if (numbers.Contains(Convert.ToString(tempPass[i])))
        {
            nCount += 1;
        }
        else if (specialCharacters.Contains(Convert.ToString(tempPass[i])))
        {
            sCount += 1;
        }
    }

    if (lCount < 3)
    {
        Console.WriteLine("Not enough lowercase characters. (At least 3)\nYou have " + lCount + "\n");
    }

    if (uCount < 4)
    {
        Console.WriteLine("Not enough uppercase characters. (At least 4)\nYou have " + uCount + "\n");
    }

    if (nCount < 3)
    {
        Console.WriteLine("Not enough numbers. (At least 3)\nYou have " + nCount + "\n");
    }

    if (sCount < 2)
    {
        Console.WriteLine("Not enough special characters. (At least 2)\nYou have " + sCount + "\n");
    }
}

//Main function window. "run" initialized at the beginning of the code
while (run){
    Menu();
}