using FrogRacer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrogRacer
{
    public class FrogData
    {
        public List<Frog> GetFrogList()
        {               
            List<Frog> frogList = new List<Frog>(); 
            frogList.Add(new Frog {Name =  "Bombay Sapphire", ImageName = "bombaysSapphire.png"});
            frogList.Add(new Frog { Name = "Chiquita", ImageName ="chiquita.png"});
            frogList.Add(new Frog { Name = "Cliff Barnes", ImageName = "cliffBarnes.png" });
            frogList.Add(new Frog { Name = "Lassie On Adventure", ImageName = "lassieOnAdventure.png" });
            frogList.Add(new Frog { Name = "Monsieur Le Puff", ImageName = "monsieurLePuff.png" });
            frogList.Add(new Frog { Name = "Mr Blue Eyes", ImageName = "mrBlueEyes.png" });
            frogList.Add(new Frog { Name = "Princess Lilian", ImageName = "princessLilian.png" });
            frogList.Add(new Frog { Name = "Robert Karlsson", ImageName = "robertKarlsson.png" });
            frogList.Add(new Frog { Name = "Speedy Gonzales", ImageName = "speedyGonzales.png" });
            frogList.Add(new Frog { Name = "Strawberry Dreams", ImageName = "strawberryDreams.png" });
            frogList.Add(new Frog { Name = "Sumo The Wrestler", ImageName = "sumoTHeWrestler.png" });
            frogList.Add(new Frog { Name = "The Guy Is On Fire", ImageName = "theGuyIsOnFire.png" });
            frogList.Add(new Frog { Name = "The Little Rascal", ImageName = "theLittleRascal.png" });
            frogList.Add(new Frog { Name = "The White Joker", ImageName = "theWhiteJoker.png" });
            frogList.Add(new Frog { Name = "Vito Corleone", ImageName = "vitoCorleone.png" });

            var randomFrogs = GetRandomFrogList(frogList);

            return randomFrogs;
        }

        private static List<Frog> GetRandomFrogList(List<Frog>froglList)
        {
            Random rnd = new Random();
            var randomFrogs = new List<Frog>();
           
            for (int i = 0; i < 5; i++)
            {
                var randomFrog = rnd.Next(0,froglList.Count);
                randomFrogs.Add(froglList[randomFrog]);
                froglList.Remove(froglList[randomFrog]);

            }

            return randomFrogs;
        }
    }
}