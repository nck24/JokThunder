// See https://aka.ms/new-console-template for more information
using Discord;
using System.IO;
namespace JokThunder
{
    class Program
    {
        static void Main(string[] args)
        {
            DiscordClient client = new DiscordClient(GetDcTolken());
            
            IReadOnlyList<DiscordRelationship> prijatelji = client.GetRelationships();

            DiscordRelationship dmPrijatelj = IzberiPrijatelja(prijatelji);
            Console.WriteLine($"\n  izbral si : {dmPrijatelj.User.Username} (pritisni Enter za nadaljevanje)");
            Console.ReadLine();

            string sporocilo = PridobiSporocilo();
            int ponovitve = PridobiStSporocil();
            int delay = PridobiDelayMili();
            
            Console.Clear();
            
            PrivateChannel dm = client.CreateDM(dmPrijatelj.User.Id);
            

            Console.WriteLine("Posiljam sporocila");
            for (int i = 0; i < ponovitve; i++)
            {
                dm.SendMessage(sporocilo);
                Thread.Sleep(delay);
            }
            Console.WriteLine("konec sem z pošiljanjem sporočil");
                 
            
            Console.ReadLine();
        }

        static string GetDcTolken(){
            if (!File.Exists("Data.txt")){
                File.Create("Data.txt").Close();
                File.AppendAllText("Data.txt", "Dc token : ");
            }
            string token;

            List<string> dataInfo = File.ReadAllLines("Data.txt").ToList();

            if (dataInfo[0] == "Dc token : "){
                Console.Write("Token :");
                token = Console.ReadLine();
                dataInfo[0] = "Dc token : " + token;
                File.WriteAllLines("Data.txt", dataInfo);
            }
            token = dataInfo[0].Substring(11);

            
            return token;
        }

        static DiscordRelationship IzberiPrijatelja(IReadOnlyList<DiscordRelationship> prijatelji){
            int poz = 0;
            Console.WriteLine("Spreminjas izbiro s puscicami gor/dol in izberes prijatelja s tipko enter\n");
            Console.WriteLine("Izberi prijatelja :\n");
            NapisiPrijatelje(prijatelji, poz);

            bool izbiranje = true;
            while (izbiranje){
                ConsoleKey gumb = Console.ReadKey(false).Key;

                if (gumb == ConsoleKey.DownArrow){
                    if ((poz + 1) > (prijatelji.Count - 1)){
                        poz = 0;
                    }else{
                        poz += 1;
                    }

                    Console.Clear();
                    Console.WriteLine("Izberi prijatelja :\n");
                    NapisiPrijatelje(prijatelji, poz);
                }else if (gumb == ConsoleKey.UpArrow){
                    if ((poz - 1) < 0){
                        poz = prijatelji.Count - 1;
                    }else{
                        poz += -1;
                    }

                    Console.Clear();
                    Console.WriteLine("Izberi prijatelja :\n");
                    NapisiPrijatelje(prijatelji, poz);
                }else if (gumb == ConsoleKey.Enter){
                    izbiranje = false;
                }
            }

            return prijatelji[poz];
            
        }

        static void NapisiPrijatelje(IReadOnlyList<DiscordRelationship> prijatelji, int poz){
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
            if (delay == null || delay < 600){
                return 600;
            }else{
                return delay;
            }
        }
    }
}