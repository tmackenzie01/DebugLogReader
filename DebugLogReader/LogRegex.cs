using System.Text.RegularExpressions;

namespace DebugLogReader
{
    public class LogRegex
    {
        // Declare and intialise these Regex here as it's costly to keep creating them
        public static Regex m_pushedRegex = new Regex("Pushed..." +
                "[0-9]+.[0-9]+.[0-9]+.(?<timestamp>[0-9]+.[0-9]+.[0-9]+.[0-9]+).(\\-\\-\\-...[0-9]+.[0-9]+.seconds..)*" +
                "Q.(?<queueCount>[0-9]+).F..?([0-9]+|ull)(,.(?<pushedPopped>[0-9]+),.[0-9]+)*$",
        RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.Multiline);

        public static Regex m_poppedRegex = new Regex("Popped..." +
            "[0-9]+.[0-9]+.[0-9]+.(?<timestamp>[0-9]+.[0-9]+.[0-9]+.[0-9]+).(\\-\\-\\-..[0-9]+.[0-9]+.seconds..)*" +
            "Q.(?<queueCount>[0-9]+).F..?((?<frameNo>[0-9]+|ull))(,.(?<pushedPopped>[0-9]+),.[0-9]+)*" +
            "(.T:A.(?<timeA>[0-9]+.[0-9]+.[0-9]+.[0-9]+).(B.(?<timeB>[0-9]+.[0-9]+.[0-9]+.[0-9]+).)*" +
            "C.(?<timeC>[0-9]+.[0-9]+.[0-9]+.[0-9]+).D.(?<timeD>[0-9]+.[0-9]+.[0-9]+.[0-9]+).)*$");

        public static Regex m_csRegex = new Regex("((?<timestamp>[0-9]+.[0-9]+.[0-9]+.[0-9]+).*[0-9]+.[0-9]+.(.)*[0-9]+.[0-9]+(.)*)*" +
            "(Write max,.WT:(?<maxTotalTimestamp>[0-9]+.[0-9]+).WW:(?<maxTotalTimestamp>[0-9]+.[0-9]+).WA:(?<maxTotalTimestamp>[0-9]+.[0-9]+).)*" +
            "$",
            RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.Multiline);

        public static Regex m_wroteDataRegex = new Regex("Wrote data( C.(?<coldstoreId>[0-9]+) P.(?<coldstorePort>[0-9]+))*$",
            RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.Multiline);

        public static Regex m_frameRegex = new Regex("Record.(?<timestamp>[0-9]+.[0-9]+.[0-9]+.[0-9]+)." +
                "(RT:(?<rtspErrorCount>[0-9]+).)*" + "\\(" +
                "(C:(?<cTimestamp1>[0-9]+.[0-9]+).O:(?<oTimestamp>[0-9]+.[0-9]+).)*" +
                "((MPEG4|H264)-AA:(?<aaTimestamp>[0-9]+.[0-9]+).)*" + "(.BB:(?<bbTimestamp>[0-9]+.[0-9]+).)*" +
                "(.(CC|CR):(?<ccTimestamp>[0-9]+.[0-9]+).)*" + "(.DD:(?<ddTimestamp>[0-9]+.[0-9]+).)*" +
                "(.EE:(?<eeTimestamp>[0-9]+.[0-9]+).)*" + "(.FF:(?<ffTimestamp>[0-9]+.[0-9]+).)*" +
                "(.GG:(?<ggTimestamp>[0-9]+.[0-9]+).)*" + "(.HH:(?<hhTimestamp>([0-9]+.[0-9]+.)*[0-9]+.[0-9]+).)*" +
                "(RV|C|(?<rvException>RVE)).(?<rvORcTimestamp>[0-9]+.[0-9]+)." + "\\)" + ".TOT.(?<totTimestamp>[0-9]+.[0-9]+).$",
                RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.Multiline);

        public static Regex m_aviRegex = new Regex("Create.(?<timestamp>[0-9]+.[0-9]+.[0-9]+.[0-9]+)" +
                "(.CR1:(?<cr1Timestamp>[0-9]+.[0-9]+).)*" + "(.CR2:(?<cr2Timestamp>[0-9]+.[0-9]+).)*" +
                "(.CR3:(?<cr3Timestamp>[0-9]+.[0-9]+).)*" +
                "(.Open max,.T:(?<maxTotalTimestamp>[0-9]+.[0-9]+).W:(?<maxTotalTimestamp>[0-9]+.[0-9]+).A:(?<maxTotalTimestamp>[0-9]+.[0-9]+).)*" +
                "(.CR4:(?<cr4Timestamp>[0-9]+.[0-9]+).)*" +
                "(.CR5:(?<cr5Timestamp>[0-9]+.[0-9]+).)*" + "(.CR6:(?<cr6Timestamp>[0-9]+.[0-9]+).)*" +
                "(.CR7:(?<cr7Timestamp>[0-9]+.[0-9]+).)*" + "(.CR8:(?<cr8Timestamp>[0-9]+.[0-9]+).)*" +
                "(.CRE:(?<creTimestamp>[0-9]+.[0-9]+).)*" + "(.CRX:(?<crxException>(.)+))*$",
                RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.Multiline);
    }
}
