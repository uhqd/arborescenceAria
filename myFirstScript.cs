using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;

[assembly: AssemblyVersion("1.0.0.1")]
[assembly: AssemblyFileVersion("1.0.0.1")]
[assembly: AssemblyInformationalVersion("1.0")]

// TODO: Uncomment the following line if the script requires write access.
// [assembly: ESAPIScript(IsWriteable = true)]
namespace VMS.TPS
{
    public class Script
    {
        public Script()
        {
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Execute(VMS.TPS.Common.Model.API.ScriptContext context /*, System.Windows.Window window, ScriptEnvironment environment*/) //
        {
            #region Basic Info
            MessageBox.Show("Open plan " + context.PlanSetup.Id);
            MessageBox.Show("Open course " + context.Course.Id);
            MessageBox.Show("Patient name " + context.Patient.Name);
            MessageBox.Show("Sex " + context.Patient.Sex);
            #endregion

            #region List of plans/courses

            string maListe = string.Empty;
            foreach (Course c in context.Patient.Courses)
                foreach (PlanSetup p in c.PlanSetups)
                {
                    maListe += "Course : " + c.Id + " Plan : " + p.Id + "\n";

                }
            MessageBox.Show(maListe);
            #endregion

            #region N fractions
            MessageBox.Show("N fractions " + context.PlanSetup.NumberOfFractions.ToString());
            #endregion

            #region  HU curve
            MessageBox.Show("Curve HU " + context.Image.Series.ImagingDeviceId);
            #endregion

            #region non empty structures
            int nStructures = context.StructureSet.Structures.Count();
            int nNonEmpty = 0;
            foreach (Structure s in context.StructureSet.Structures)
            {
                if (!s.IsEmpty)
                    nNonEmpty++;

            }
            MessageBox.Show(nStructures.ToString() + " structures and " + nNonEmpty.ToString() + " are non empty");
            #endregion

            #region table of tolerance
            string tolTable = "";
            foreach (Beam f in context.PlanSetup.Beams)
            {
                if (!f.IsSetupField)
                {
                    tolTable = f.ToleranceTableLabel.ToUpper();
                    MessageBox.Show("table of tolerance of " + f.Id + " : " + tolTable);
                    break;
                }
            }
            #endregion

            #region MLC mean gap
            Beam b = context.PlanSetup.Beams.FirstOrDefault();
            MessageBox.Show("Processing " + b.Id);
            double meang = 0;
            double totalMeanLeafGap = 0;
            int openLeaves = 0;
            foreach (ControlPoint cp in b.ControlPoints)
            {
                double meanLGforOneCP = 0.0;

                for (int i = 0; i < cp.LeafPositions.GetLength(1); i++) // Parcours des lames
                {
                    if (cp.LeafPositions[0, i] != cp.LeafPositions[1, i])
                    {
                        openLeaves++;
                        meanLGforOneCP += cp.LeafPositions[1, i] - cp.LeafPositions[0, i];
                    }
                }
                meanLGforOneCP = meanLGforOneCP / openLeaves;
                totalMeanLeafGap += meanLGforOneCP;

            }
            meang = totalMeanLeafGap / b.ControlPoints.Count;
            MessageBox.Show("Mean GAP " + meang.ToString("F2"));
            #endregion
        }
    }
}
