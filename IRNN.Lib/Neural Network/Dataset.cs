namespace IRNN {

    public class DataSet {

        #region -- Properties --

        public double[] Values {
            get; set;
        }

        public double[] Targets {
            get; set;
        }

        #endregion -- Properties --

        #region -- Constructor --

        public DataSet(double[] values, double[] targets) {
            Values = values;
            Targets = targets;
        }

        #endregion -- Constructor --
    }
}