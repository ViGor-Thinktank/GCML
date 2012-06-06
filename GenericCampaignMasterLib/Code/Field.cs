using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterLib
{
    
    public class Field_Schachbrett : Field
    {
        private int m_width = -1;
        private int m_height = -1;

        public class clsSektorKoordinaten_Schachbrett : clsSektorKoordinaten
        {
            public int X = -1;
            public int Y = -1;
            private clsSektorKoordinaten_Schachbrett objSektorKoordSchach;

            public clsSektorKoordinaten_Schachbrett(int newX, int newY)
            {
                this.X = newX;
                this.Y = newY;
            }

            public clsSektorKoordinaten_Schachbrett(clsSektorKoordinaten_Schachbrett objSektorKoordSchach)
            {
                this.X = objSektorKoordSchach.X;
                this.Y = objSektorKoordSchach.Y;
            }

            public override bool Equals(clsSektorKoordinaten other)
            {
                clsSektorKoordinaten_Schachbrett otherCast = (clsSektorKoordinaten_Schachbrett)other;
                return (this.X == otherCast.X && this.Y == otherCast.Y);    
            }

            public static clsSektorKoordinaten_Schachbrett operator +(clsSektorKoordinaten_Schachbrett c1, clsSektorKoordinaten_Schachbrett c2)
            {
                return new clsSektorKoordinaten_Schachbrett(c1.X + c2.X, c1.Y + c2.Y);
            }
            
            public override string uniqueIDstr()
            {
                return X.ToString() + "|" + Y.ToString();
            }

            
        }

        public Field_Schachbrett(int width, int height)
        {
            m_width = width;
            m_height = height;
                
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Sektor newSek = new Sektor(this.ListSektors.Count.ToString());
                    newSek.objSektorKoord = new clsSektorKoordinaten_Schachbrett(i, j);
                    
                    newSek.onUnitEnteredSektor += new EventHandler(newSek_onUnitEnteredSektor);
                    newSek.onUnitLeftSektor += new EventHandler(newSek_onUnitLeftSektor); 
                    
                    this.ListSektors.Add(newSek.strUniqueID, newSek);
                }
            }
        }

        
        public override Sektor get(string strSektorID)
        {
            return this.ListSektors[strSektorID];
        }

        public override Sektor get(Field.clsSektorKoordinaten objSektorKoord)
        {
            return ListSektors[objSektorKoord.uniqueIDstr()];
        }

        public override bool checkKoordsValid(Field.clsSektorKoordinaten objSektorKoord)
        {
            clsSektorKoordinaten_Schachbrett objSektorKoordSchach = (clsSektorKoordinaten_Schachbrett)objSektorKoord;
            return objSektorKoordSchach.X < this.m_height && objSektorKoordSchach.Y < m_width;
        }

        public override List<Field.clsSektorKoordinaten> getMoveVektors()
        {
            List<Field.clsSektorKoordinaten> lisVektors = new List<clsSektorKoordinaten>();
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    lisVektors.Add(new clsSektorKoordinaten_Schachbrett(i, j));
                }
            }
            return lisVektors;
        }

        

        public override Sektor move(Sektor aktSek, Field.clsSektorKoordinaten Vektor)
        {
             clsSektorKoordinaten_Schachbrett objZielSektorKoordSchach = (clsSektorKoordinaten_Schachbrett)aktSek.objSektorKoord + (clsSektorKoordinaten_Schachbrett)Vektor;

            if (ListSektors.ContainsKey(objZielSektorKoordSchach.uniqueIDstr()))
                return ListSektors[objZielSektorKoordSchach.uniqueIDstr()];
            else
                return null;
        }

        protected override void setNullSektor()
        {
            this.nullSektor = new clsSektorKoordinaten_Schachbrett(0, 0);
        }
    }

    public class Field_Schlauch : Field
    {
        
        public Field_Schlauch(List<Sektor> lisNewSektors)
        { 
            foreach (Sektor sekNew in lisNewSektors)
            {
                this.ListSektors.Add(new clsSektorKoordinaten_Schlauch(sekNew.Id).uniqueIDstr(), sekNew);
            }
        }

        public Field_Schlauch(int p)
        {
            for (int i = 0; i < p; i++)
            {
                Sektor newSek = new Sektor(this.ListSektors.Count.ToString());
                newSek.objSektorKoord = new clsSektorKoordinaten_Schlauch(i);
                newSek.onUnitEnteredSektor += new EventHandler(newSek_onUnitEnteredSektor);
                newSek.onUnitLeftSektor += new EventHandler(newSek_onUnitLeftSektor); 
                    
                this.ListSektors.Add(newSek.objSektorKoord.uniqueIDstr(), newSek);
            }
        }

        protected override void setNullSektor()
        {
            this.nullSektor = new clsSektorKoordinaten_Schlauch(1);
        }

        
        public class clsSektorKoordinaten_Schlauch : clsSektorKoordinaten
        {
            public int X = -1;

            public clsSektorKoordinaten_Schlauch(string newX)
            {
                this.X = Convert.ToInt32(newX);
            }
            
            public clsSektorKoordinaten_Schlauch(int newX)
            {
                this.X = newX;
            }

            

            public override bool Equals(clsSektorKoordinaten other)
            {
                clsSektorKoordinaten_Schlauch otherCast = (clsSektorKoordinaten_Schlauch)other;
                return (this.X == otherCast.X);
            }

            public override string uniqueIDstr()
            {
                return this.X.ToString();
            }
        }

        public override Sektor get(string  strSektorID)
        {
            foreach (Sektor aktSektor in ListSektors.Values)
            {
                if (aktSektor.Id == strSektorID)
                    return aktSektor;
            }

            return null;
        }

        public override Sektor get(Field.clsSektorKoordinaten objSektorKoord)
        {
            return ListSektors[objSektorKoord.uniqueIDstr()];
        }

        public override bool checkKoordsValid(Field.clsSektorKoordinaten objSektorKoord)
        {
            return ((clsSektorKoordinaten_Schlauch)objSektorKoord).X < this.ListSektors.Count;
        }

        public override List<Field.clsSektorKoordinaten> getMoveVektors()
        {
            List<Field.clsSektorKoordinaten> lisVektors = new List<clsSektorKoordinaten>();
            for (int i = -1; i <=1; i++)
            {
                lisVektors.Add(new clsSektorKoordinaten_Schlauch(i));
            }
            return lisVektors;
        }

        public override Sektor move(Sektor aktSek, Field.clsSektorKoordinaten Vektor)
        {
            int newID = ((clsSektorKoordinaten_Schlauch)aktSek.objSektorKoord).X + ((clsSektorKoordinaten_Schlauch)Vektor).X;
            foreach (Sektor aktSektor in ListSektors.Values)
            {
                if (aktSektor.Id == newID.ToString())
                    return aktSektor;
            }

            return null;
        }

       
    }

    public abstract class Field : IEquatable<Field>
    {
        public abstract class clsSektorKoordinaten
        {
           public abstract bool Equals(clsSektorKoordinaten other);

           public abstract string uniqueIDstr();
            
        }

        public Field()
        {
            setNullSektor();
        }
        
        public clsSektorKoordinaten nullSektor = null;
        protected abstract void setNullSektor();


        public abstract Sektor get(string strSektorID);
        public abstract Sektor get(clsSektorKoordinaten objSektorKoord);

        public abstract bool checkKoordsValid(clsSektorKoordinaten objSektorKoord);

        protected Dictionary<string, Sektor> ListSektors = new Dictionary<string, Sektor>();
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
        public abstract List<Field.clsSektorKoordinaten> getMoveVektors();

        public int Id { get; set; }
        
        public bool Equals(Field other)
        {
            return (this.Id == other.Id);
        }

        public delegate void delStatus(string strText);
        
        public event delStatus onFieldStatus;


        protected  void newSek_onUnitEnteredSektor(object sender, EventArgs e)
        {
            Sektor SekSender = (Sektor)sender;
            if (onFieldStatus != null)
                onFieldStatus("UnitEnter " + SekSender.strUniqueID);
        }

        protected void newSek_onUnitLeftSektor(object sender, EventArgs e)
        {
            Sektor SekSender = (Sektor)sender;
            if (onFieldStatus != null)
                onFieldStatus("UnitLeft " + SekSender.strUniqueID);
        }

        
    }
    
 
}
