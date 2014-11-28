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
            var frogList = new List<Frog>();
            //C:\Users\David\Documents\GitHub\FrogRacer\FrogRacer\Content\Images\bombaysSapphire.png
            frogList.Add(new Frog {Name =  "Bombay Sapphire", ImageName = "bombaysSapphire.png"});
            frogList.Add(new Frog { Name = "Chiquita", ImageName ="chiquita.png"});
            frogList.Add(new Frog { Name = "Cliff Barnes", ImageName = "cliffBarnes.png" });
            frogList.Add(new Frog { Name = "Lassie On Adventure", ImageName = "lassieOnAdventure.png" });
            frogList.Add(new Frog { Name = "Monsieur Le Puff", ImageName = "monsieurLePuff.png" });
            frogList.Add(new Frog { Name = "Mr Blue Eyes", ImageName = "mrBlueEyes.png" });
            frogList.Add(new Frog { Name = "Princess Lillian", ImageName = "princessLilian.png" });
            frogList.Add(new Frog { Name = "Robert Karlsson", ImageName = "robertKarlsson.png" });
            frogList.Add(new Frog { Name = "Speedy Gonzales", ImageName = "speedyGonzales.png" });
            frogList.Add(new Frog { Name = "Strawberry Dreams", ImageName = "strawberryDreams.png" });
            frogList.Add(new Frog { Name = "Sumo The Wrestler", ImageName = "sumoTHeWrestler.png" });
            frogList.Add(new Frog { Name = "The Guy Is On Fire", ImageName = "theGuyIsOnFire.png" });
            frogList.Add(new Frog { Name = "The Listtle Rascal", ImageName = "theLittleRascal.png" });
            frogList.Add(new Frog { Name = "The White Joker", ImageName = "theWhiteJoker.png" });
            frogList.Add(new Frog { Name = "Vito Corleone", ImageName = "vitoCorleone.png" });

            return frogList;
        }
    }
}