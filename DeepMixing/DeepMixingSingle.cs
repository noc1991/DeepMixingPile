using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepMixing
{
    internal class DeepMixingSingle
    {

        int UploadTime = 60;        // 加载龄期（路基填筑至75%高度的天数），单位：day
        double qdm_spec = 0.862;    // 水泥土的28天无侧限抗压强度，MPa
        double fr = 0.8;            // 无侧限峰值强度与受限大应变之间的差异系数，通长 0.65~0.9，路基建议为 0.8

        double Length;
        double Diameter;
        string ConstructMethod;     // 施工方法，可选湿法 "wet"，干法 "dry"

        double fc;                  // 强度放大系数（根据加载龄期对28天强度进行放大）
        double Sdm;                 // 抗剪强度设计值，MPa
        public double Edm;          // 搅拌桩的弹性模量，MPa


        public DeepMixingSingle(double length, double dia, string constructMethod = "wet")
        {
            Length = length;
            Diameter = dia;
            ConstructMethod = constructMethod;

            CalShearStrength();
            CalModulus();
        }

        public void CalShearStrength()
        {
            // 根据规范 6.1.3（Step3.1~2，P53），计算搅拌桩抗剪强度

            fc = 0.187 * Math.Log(UploadTime) + 0.375;
            Sdm = 1 / 2 * fr * fc * qdm_spec;
        }

        public void CalModulus()
        {
            // 根据规范 6.1.3（Step3.4，P55），计算搅拌桩杨氏模量

            if (ConstructMethod == "wet")
                Edm = 300 * qdm_spec;
            else if (ConstructMethod == "dry")
                Edm = 150 * qdm_spec;
        }

        public static double GetFv(double fos, double cov=0.5, double pdm=0.8)
        {
            // 根据规范 6.1.3（Step3.3，P53），计算 fv

            int r = (int)(10 * cov) - 4;
            int c = (int)(10 * pdm) - 7;

            double[,] fv;

            if (fos == 1.2)
            {
                fv = new double[,] {
                    { 0.93, 1.05, 1.25},
                    {0.88, 1.02, 1.26 },
                    {0.83, 0.99, 1.27 }
                };
                return fv[r, c];

            }
            else if (fos == 1.3)
            {
                fv = new double[,] {
                    {0.89, 1.01, 1.19 },
                    {0.82, 0.95, 1.17 },
                    {0.75, 0.9, 1.15 }
                };
                return fv[r, c];

            }

            else if (fos == 1.4)
            {
                fv = new double[,]
                {
                    {0.85, 0.97, 1.14 },
                    {0.76, 0.89, 1.09 },
                    {0.69, 0.82, 1.05 }
                };
                return fv[r, c];

            }

            else if (fos == 1.5)
            {
                fv = new double[,]
                {
                    {0.82, 0.93, 1.1 },
                    {0.72, 0.83, 1.03 },
                    {0.63, 0.75, 0.96 }
                };
                return fv[r, c];

            }

            else if (fos == 1.6)
            {
                fv = new double[,]
                {
                    {0.79, 0.9, 1.06 },
                    {0.68, 0.79, 0.97 },
                    {0.58, 0.69, 0.89 }
                };
                return fv[r, c];

            }

            else
                throw new Exception();
        }


    }
}
