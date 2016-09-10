using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;


namespace SepaKonverter
{
    public class OrganisationBO
    {
        private OrganisationDAO organisationDAO;
        private DataTable dataTable;

        public OrganisationBO()
        {
             organisationDAO = new OrganisationDAO();  
        }
        
        public OrganisationVO getOrganisationById(string _id)
        {
            OrganisationVO organisationVO = new OrganisationVO();
            dataTable = new DataTable();
            dataTable = organisationDAO.searchById(_id);

            foreach (DataRow dr in dataTable.Rows)
            {
                organisationVO.OrganisationsNr= dr[0].ToString();
                organisationVO.Name = dr[1].ToString();
                organisationVO.GläubigerID = dr[2].ToString();
                organisationVO.BIC = dr[3].ToString();
                organisationVO.IBAN = dr[4].ToString();
            }
            return organisationVO;
        }

        public int getAnzahlOrganisationen()
        {
            return organisationDAO.AnzahlOrganisationen();
        }

        public OrganisationVO[] getOrganisationenDetails()
        {
            int i = 0;
            dataTable = new DataTable();
            
            dataTable = organisationDAO.OrganisationenDetails();
            OrganisationVO[] orgVO = new OrganisationVO[dataTable.Rows.Count];

            foreach (DataRow dr in dataTable.Rows)
            {
                orgVO[i] = new OrganisationVO(dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString());
                i++;
            }
            return orgVO;
        }
    }
}
