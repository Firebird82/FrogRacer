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

            frogList.Add(new Frog {Name =  "Oljade Blixten"});
            frogList.Add(new Frog { Name = "Flygande Skuttis" });

            return frogList;
        }
    }
}