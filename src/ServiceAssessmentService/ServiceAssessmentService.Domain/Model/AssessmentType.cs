using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAssessmentService.Domain.Model;
public record AssessmentType(string Name)
{
    public static readonly AssessmentType PeerReview = new("Peer Review");
    public static readonly AssessmentType MockAssessment = new("Mock Assessment");
    public static readonly AssessmentType Continuous = new("Continuous");
    public static readonly AssessmentType DepartmentalInternal = new("Internal (DfE)");
    public static readonly AssessmentType CrossGovernment = new("Cross-Government (CDDO)");
    //public static readonly AssessmentType ReAssessment = new("Re-Assessment");

    public static readonly AssessmentType[] All =
    {
        PeerReview,
        MockAssessment,
        Continuous,
        DepartmentalInternal,
        CrossGovernment,
        //ReAssessment,
    };

    public static AssessmentType? FromName(string name)
    {
        return All.SingleOrDefault(x => x.Name == name);
    }
}
