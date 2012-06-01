using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterLib
{
    
    public class Field_Schachbrett : clsSektorCollection
    {
        private int m_width = -1;
        private int m_height = -1;

        public class clsSektorKoordinaten_Schachbrett : clsSektorKoordinaten

        {
            public int X = -1;
            public int Y = -1;

            public clsSektorKoordinaten_Schachbrett(int newX, int newY)
            {
                this.X = newX;
                this.Y = newY;
            }

            public override bool Equals(clsSektorKoordinaten other)
            {
                clsSektorKoordinaten_Schachbrett otherCast = (clsSektorKoordinaten_Schachbrett)other;
                return (this.X == otherCast.X && this.Y == otherCast.Y);    
            }


            
        }

        public Field_Schachbrett(int width, int height)
        {
            m_width = width;
            m_height = height;
                
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; i < height; j++)
                {
                    Sektor newSek = new Sektor(this.ListSektors.Count);
                    newSek.objSektorKoord = new clsSektorKoordinaten_Schachbrett(i, j);
                    this.ListSektors.Add(newSek.objSektorKoord, newSek);
                }
            }
        }

        public override Sektor get(int intSektorID)
        {
            throw new NotImplementedException();
        }

        public override Sektor get(clsSektorCollection.clsSektorKoordinaten objSektorKoord)
        {
            return ListSektors[objSektorKoord];
        }

        public override bool checkKoordsValid(clsSektorCollection.clsSektorKoordinaten objSektorKoord)
        {
            clsSektorKoordinaten_Schachbrett objSektorKoordSchach = (clsSektorKoordinaten_Schachbrett)objSektorKoord;
            return objSektorKoordSchach.X < this.m_height && objSektorKoordSchach.Y < m_width;
        }

        public override List<clsSektorCollection.clsSektorKoordinaten> getMoveVektors(int range)
        {
            List<clsSektorCollection.clsSektorKoordinaten> lisVektors = new List<clsSektorKoordinaten>();
            for (int i = -range; i <= range; i++)
            {
                for (int j = -range; i <= range; i++)
                {
                    lisVektors.Add(new clsSektorKoordinaten_Schachbrett(i, j));
                }
            }
            return lisVektors;
        }

        public override Sektor move(Sektor aktSek, clsSektorCollection.clsSektorKoordinaten Vektor)
        {
            clsSektorKoordinaten_Schachbrett objSektorKoordSchach = (clsSektorKoordinaten_Schachbrett)aktSek.objSektorKoord;
            clsSektorKoordinaten_Schachbrett objVektorKoordSchach = (clsSektorKoordinaten_Schachbrett)Vektor;

            objSektorKoordSchach.X += objVektorKoordSchach.X;
            objSektorKoordSchach.Y += objVektorKoordSchach.Y;

            if (ListSektors.ContainsKey(objSektorKoordSchach))
                return ListSektors[objSektorKoordSchach];
            else
                return null;
        }
    }

    public class Field_Schlauch : clsSektorCollection
    {
        public Field_Schlauch(List<Sektor> lisNewSektors)
        { 
            foreach (Sektor sekNew in lisNewSektors)
            {
                this.ListSektors.Add(new clsSektorKoordinaten_Schlauch(sekNew.Id), sekNew);
            }
        }

        public class clsSektorKoordinaten_Schlauch : clsSektorKoordinaten
        {
            public int X = -1;

            public clsSektorKoordinaten_Schlauch(int newX)
            {
                this.X = newX;
            }

            public override bool Equals(clsSektorKoordinaten other)
            {
                clsSektorKoordinaten_Schlauch otherCast = (clsSektorKoordinaten_Schlauch)other;
                return (this.X == otherCast.X);
            }
}

        public override Sektor get(int intSektorID)
        {
            foreach (Sektor aktSektor in ListSektors.Values)
            {
                if (aktSektor.Id == intSektorID)
                    return aktSektor;
            }

            return null;
        }

        public override Sektor get(clsSektorCollection.clsSektorKoordinaten objSektorKoord)
        {
            return ListSektors[(clsSektorKoordinaten_Schlauch)objSektorKoord];
        }

        public override bool checkKoordsValid(clsSektorCollection.clsSektorKoordinaten objSektorKoord)
        {
            return ((clsSektorKoordinaten_Schlauch)objSektorKoord).X < this.ListSektors.Count;
        }

        public override List<clsSektorCollection.clsSektorKoordinaten> getMoveVektors(int range)
        {
            List<clsSektorCollection.clsSektorKoordinaten> lisVektors = new List<clsSektorKoordinaten>();
            for (int i = -range; i <=range; i++)
            {
                lisVektors.Add(new clsSektorKoordinaten_Schlauch(i));
            }
            return lisVektors;
        }

        public override Sektor move(Sektor aktSek, clsSektorCollection.clsSektorKoordinaten Vektor)
        {
            int newID = aktSek.Id + ((clsSektorKoordinaten_Schlauch)Vektor).X;
            foreach (Sektor aktSektor in ListSektors.Values)
            {
                if (aktSektor.Id == newID)
                    return aktSektor;
            }

            return null;
        }
    }

    public abstract class clsSektorCollection
    {
        public abstract class clsSektorKoordinaten
        {
            public abstract bool Equals(clsSektorKoordinaten other);
        }
        
        public abstract Sektor get(int intSektorID);
        public abstract Sektor get(clsSektorKoordinaten objSektorKoord);


        public abstract bool checkKoordsValid(clsSektorKoordinaten objSektorKoord);

        protected Dictionary<clsSektorKoordinaten, Sektor> ListSektors = new Dictionary<clsSektorKoordinaten, Sektor>();
        public Sektor getSektorForUnit(Code.Unit.IUnit u)
        {
            // Erstmal so gelöst. Geht besser.      
            foreach (Sektor s in ListSektors.Values)
            {
                if (s.ListUnits.Contains(u))
                    return s;
            }

            return null;
        }

        public abstract Sektor move(Sektor aktSek, clsSektorKoordinaten Vektor);
        public abstract List<clsSektorCollection.clsSektorKoordinaten> getMoveVektors(int range);
        
    }
    
    public class Field : IEquatable<Field>
    {

        public Field(clsSektorCollection newSektorCollection)
        {
            this.objSektorCollection = newSektorCollection;
        }
        public int Id { get; set; }
        public clsSektorCollection objSektorCollection;
            
	        
        #region IEquatable<Field> Member

        // TODO: Nur Temporär um die Testklasse auszuprobieren.
        // Muss die Sektoren vergleichen.
        public bool Equals(Field other)
        {
            if (this.Id == other.Id)
                return true;
            else
                return false;
        }

        #endregion
    }
}
