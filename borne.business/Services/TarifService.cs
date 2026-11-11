using borne.core.Entities;
using borne.core.Enums;
using borne.core.IInfra;
using System;
using System.Collections.Generic;
using System.Text;

namespace borne.business.Services
{
    public class TarifService
    {

        private readonly ITarifDAO tarifDAO;
        public TarifService(ITarifDAO tarifDAO)
        {
            this.tarifDAO = tarifDAO;
        }

        public async Task<List<Tarif>> GetAllTarifs() {
            return await tarifDAO.GetAll();
        }

        public async Task<List<Tarif>> GetTarifsByEnergie(TypeEnergie typeEnergie)
        {
            return await tarifDAO.GetByEnergie(typeEnergie);
        }

        public async Task AddTarif(Tarif tarif)
        {
            if(tarif == null)
            {
                throw new ArgumentNullException(nameof(tarif));
            }
            if(!Enum.IsDefined(typeof(TypeEnergie), tarif.TypeEnergie))
            {
                throw new ArgumentException("Le Type d'energie specifiee est invalide", nameof(tarif));
            }

            await tarifDAO.Add(tarif);
        }

        public async Task UpdateTarif(Tarif tarif)
        {
            if (tarif == null)
            {
                throw new ArgumentNullException(nameof(tarif));
            }

            Tarif existingTarif = await tarifDAO.GetById(tarif.IdTarif);
            if(existingTarif == null)
            {
                throw new ArgumentException("Le Tarif n'existe pas");
            }

            if (!Enum.IsDefined(typeof(TypeEnergie), tarif.TypeEnergie))
            {
                throw new ArgumentException("Le Type d'energie specifiee est invalide", nameof(tarif));
            }
            await tarifDAO.Update(tarif);
        }

        public async Task DeleteTarif(int idTarif)
        {
            Tarif existingTarif = await tarifDAO.GetById(idTarif);
            if(existingTarif == null)
            {
                throw new ArgumentException("Le Tarif n'existe pas ! ");
                
            }
            await tarifDAO.Delete(existingTarif);
        }
    }
}
