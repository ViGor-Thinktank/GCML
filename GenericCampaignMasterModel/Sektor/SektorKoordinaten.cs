﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace GenericCampaignMasterModel
{
    public class clsSektorKoordinaten
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        private List<int> m_lstIntPosition;
        
        public List<int> Position
        {
            get
            {
                return m_lstIntPosition;
            }

            set
            {
                m_lstIntPosition = value;
            }
        }

        public clsSektorKoordinaten()
        {

        }

        public clsSektorKoordinaten(int x = 0, int y = 0, int z = 0)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.m_lstIntPosition = new List<int> { x, y, z };
        }

      
        public virtual bool Equals(clsSektorKoordinaten other)
        {
            if (this.Position == other.Position)
                return true;
            else
                return false;
        }

        public virtual string uniqueIDstr()
        {
            return clsSektorKoordinaten.getKoordsFormatted(this.X, this.Y, this.Z);
        }

        /// <summary>
        /// Formatiert xyz Koordinaten in einem String. Soll einheitliches Format sicherstellen.
        /// </summary>
        public static string getKoordsFormatted(int x = 0, int y = 0, int z = 0)
        {
               return String.Format("{0:D2}_{1:D2}_{2:D2}", x, y, z);
        }

        public static clsSektorKoordinaten operator +(clsSektorKoordinaten c1, clsSektorKoordinaten c2)
        {
            return new clsSektorKoordinaten(c1.X + c2.X, c1.Y + c2.Y, c1.Z + c2.Z);
        }

       
    }
}
