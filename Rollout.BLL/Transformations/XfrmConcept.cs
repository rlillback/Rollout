using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rollout.BLL
{
    public static class XfrmConcept
    {
        public static Concept CSVtoConcept( ConceptCSV csv )
        {
            Concept con = new Concept();

            return con;
        } // CSVtoConcept

        public static ConceptCSV ConceptToCSV( Concept con )
        {
            //TODO: Do I ever need this? 
            ConceptCSV csv = new ConceptCSV();
            return csv;
        } // ConceptToCSV 
    }
}
