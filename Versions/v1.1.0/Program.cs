// See https://aka.ms/new-console-template for more information
using Discord;
using System.Collections;
using System.IO;
namespace JokThunder
{
    class Program
    {
        static void Main(string[] args)
        {
            DiscordClient client = new DiscordClient(GetDcTolken());
            Console.Clear();
            
            IReadOnlyList<DiscordRelationship> prijatelji = client.GetRelationships();
            Span<DiscordRelationship> sorted = new Span<DiscordRelationship>(prijatelji.ToArray());
            sorted.Sort((x, y) => x.User.Username.CompareTo(y.User.Username));


            DiscordRelationship dmPrijatelj = IzberiPrijatelja(sorted);
            Console.WriteLine($"\n  izbral si : {dmPrijatelj.User.Username} (pritisni Enter za nadaljevanje)");
            Console.ReadLine();

            string sporocilo = PridobiSporocilo();
            int ponovitve = PridobiStSporocil();
            int delay = PridobiDelayMili();
            
            Console.Clear();
            
            PrivateChannel dm = client.CreateDM(dmPrijatelj.User.Id);
            
            int spremembeProgresBara = 20;
            Console.CursorVisible = false;
            Console.WriteLine("Posiljam sporocila");
            WriteProgresBar(spremembeProgresBara, 2);
            for (int i = 0; i < ponovitve; i++)
            {
                dm.SendMessage(sporocilo);
                StartProgressBar(ponovitve, i, spremembeProgresBara, 2);
                Thread.Sleep(delay);
            }
            StartProgressBar(ponovitve, ponovitve, spremembeProgresBara, 2);
            Console.CursorVisible = true;
            Console.SetCursorPosition(0, 3);

            Console.WriteLine("\nkonec sem z pošiljanjem sporočil");
                 
            
            Console.ReadLine();
        }

        static string GetDcTolken(){
            if (!File.Exists("Data.txt")){
                File.Create("Data.txt").Close();
                File.AppendAllText("Data.txt", "Dc token : ");
            }
            string? token;

            List<string> dataInfo = File.ReadAllLines("Data.txt").ToList();

            if (dataInfo[0] == "Dc token : "){
                Console.Write("Token : ");
                token = Console.ReadLine();
                dataInfo[0] = "Dc token : " + token;
                File.WriteAllLines("Data.txt", dataInfo);
            }
            token = dataInfo[0].Substring(11);

            
            return token;
        }
        
        static DiscordRelationship IzberiPrijatelja(Span<DiscordRelationship> prijatelji){
            int poz = 0;
            Console.WriteLine("Spreminjas izbiro s puscicami gor/dol in izberes prijatelja s tipko enter\n");
            Console.WriteLine("Izberi prijatelja :\n");
            NapisiPrijatelje(prijatelji, poz);

            bool izbiranje = true;
            while (izbiranje){
                ConsoleKey gumb = Console.ReadKey(false).Key;

                if (gumb == ConsoleKey.DownArrow){
                    if ((poz + 1) > (prijatelji.Length - 1)){
                        poz = 0;
                    }else{
                        poz += 1;
                    }

                    Console.Clear();
                    Console.WriteLine("Izberi prijatelja :\n");
                    NapisiPrijatelje(prijatelji, poz);
                }else if (gumb == ConsoleKey.UpArrow){
                    if ((poz - 1) < 0){
                        poz = prijatelji.Length - 1;
                    }else{
                        poz += -1;
                    }

                    Console.Clear();
                    Console.WriteLine("Izberi prijatelja :\n");
                    NapisiPrijatelje(prijatelji, poz);
                }else if (gumb == ConsoleKey.Enter){
                    izbiranje = false;
                }
                Thread.Sleep(100);
            }

            return prijatelji[poz];
            
        }

        static void NapisiPrijatelje(Span<DiscordRelationship> prijatelji, int poz){
            foreach (DiscordRelationship rl in prijatelji)
            {
                if (prijatelji[poz] == rl){
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine("  >> " + rl.User.Username);
                    Console.ResetColor();
                }
                else{
                    Console.WriteLine("     " + rl.User.Username);
                }
            }
        }

        static string PridobiSporocilo(){
            Console.Write("  Kaksno sporocilo hoces poslati : ");
            return Console.ReadLine();
        }
    
        static int PridobiStSporocil(){
            Console.Write("  Koliko krat naj poslem : ");
            return Convert.ToInt32(Console.ReadLine());
        }
    
        static int PridobiDelayMili(){
            Console.Write("  Casovni zamik med sporocili(v milisekundah, min 600) : ");
            int delay = Convert.ToInt32(Console.ReadLine());
            if (delay < 600){
                return 600;
            }
            else{
                return delay;
            }
        }
    
        static void StartProgressBar(int stDm, int zePoslano, int stSprememb, int linijaBara){
            double percent = (double)zePoslano / (double)stDm;
            int pozCrtice = Convert.ToInt32(Math.Round((stSprememb - 1) * percent)) + 1;
            Console.SetCursorPosition(1, linijaBara);
            for (int i = 0; i < pozCrtice; i++){
                Console.Write("|");
            }
        }
    
        static void WriteProgresBar(int spremembe, int linija){
            Console.SetCursorPosition(0, linija);
            Console.Write("[");
            for (int i = 0; i < spremembe; i++){
                Console.Write("-");
            }
            Console.WriteLine("]");
        }
    }
}