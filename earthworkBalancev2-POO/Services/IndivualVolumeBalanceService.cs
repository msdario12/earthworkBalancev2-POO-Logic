using Autodesk.Civil.DatabaseServices;
using earthworkBalancev2_POO.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace earthworkBalancev2_POO.Services
{
    public class IndivualVolumeBalanceService
    {
        // Initializate repository
        private ICivil3DRepository _civil3DRepository;

        public IndivualVolumeBalanceService(ICivil3DRepository civil3DRepository, string volumeSurfaceName)
        {
            _civil3DRepository = civil3DRepository ?? throw new ArgumentNullException(nameof(civil3DRepository));

            Surface volumeSurface = _civil3DRepository.GetSurfaceByName(volumeSurfaceName);
            // Get last 4 digit
            string last4Digit = volumeSurfaceName.Substring(volumeSurfaceName.Length - 4);
            // Search a featureLine with the same last digits.
            List<FeatureLine> featureLines = _civil3DRepository.GetAllFeatureLines();
            FeatureLine featureLine = featureLines.FirstOrDefault(x => x.Name.Contains(last4Digit));
            if (featureLine == null)
            {
                return;
                throw new Exception($"Feature line with last 4 digits {last4Digit} not found.");
            }

            // Get net volume balance
            double adjustedNetVolume = (volumeSurface as TinVolumeSurface).GetVolumeProperties().AdjustedNetVolume;
            double meanVolumeElevation = (volumeSurface as TinSurface).GetGeneralProperties().MeanElevation;

            double actualFeatureLineElevation = featureLine.StartPoint.Z;

            double iteration = 0;
            while( iteration < 0 && )



        }

    }
}
