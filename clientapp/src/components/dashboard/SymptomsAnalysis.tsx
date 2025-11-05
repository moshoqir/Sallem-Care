import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { CheckCircle, XCircle, Stethoscope } from "lucide-react";

interface Symptom {
  name: string;
  present: boolean;
  severity?: "mild" | "moderate" | "severe";
  duration?: string;
}

interface SymptomsAnalysisProps {
  chiefComplaint: {
    complaint: string;
    duration: string;
    bodySystem: string;
  };
  symptoms: Symptom[];
}

export const SymptomsAnalysis = ({ chiefComplaint, symptoms }: SymptomsAnalysisProps) => {
  const presentSymptoms = symptoms.filter(s => s.present);
  const absentSymptoms = symptoms.filter(s => !s.present);

  const getSeverityColor = (severity?: string) => {
    switch (severity) {
      case "mild": return "bg-medical-success text-medical-success-foreground";
      case "moderate": return "bg-medical-warning text-medical-warning-foreground";
      case "severe": return "bg-medical-danger text-medical-danger-foreground";
      default: return "bg-muted text-muted-foreground";
    }
  };

  return (
    <div className="space-y-6">
      {/* Chief Complaint */}
      <Card className="shadow-card border-l-4 border-l-primary">
        <CardHeader className="pb-3">
          <CardTitle className="flex items-center text-foreground">
            <Stethoscope className="h-5 w-5 mr-2 text-primary" />
            Chief Complaint
          </CardTitle>
        </CardHeader>
        <CardContent>
          <div className="space-y-3">
            <div>
              <h3 className="font-semibold text-lg text-foreground">{chiefComplaint.complaint}</h3>
              <p className="text-muted-foreground">Duration: {chiefComplaint.duration}</p>
            </div>
            <Badge className="bg-primary text-primary-foreground">
              {chiefComplaint.bodySystem} System
            </Badge>
          </div>
        </CardContent>
      </Card>

      <div className="grid gap-6 md:grid-cols-2">
        {/* Present Symptoms */}
        <Card className="shadow-card">
          <CardHeader className="pb-3">
            <CardTitle className="flex items-center text-foreground">
              <CheckCircle className="h-5 w-5 mr-2 text-medical-success" />
              Present Symptoms ({presentSymptoms.length})
            </CardTitle>
          </CardHeader>
          <CardContent>
            <div className="space-y-3">
              {presentSymptoms.length === 0 ? (
                <p className="text-muted-foreground text-center py-4">No symptoms reported</p>
              ) : (
                presentSymptoms.map((symptom, index) => (
                  <div key={index} className="flex items-center justify-between p-3 border border-border rounded-lg bg-medical-success/5">
                    <div>
                      <span className="font-medium text-foreground">{symptom.name}</span>
                      {symptom.duration && (
                        <p className="text-sm text-muted-foreground">{symptom.duration}</p>
                      )}
                    </div>
                    {symptom.severity && (
                      <Badge className={`text-xs ${getSeverityColor(symptom.severity)}`}>
                        {symptom.severity}
                      </Badge>
                    )}
                  </div>
                ))
              )}
            </div>
          </CardContent>
        </Card>

        {/* Absent Symptoms */}
        <Card className="shadow-card">
          <CardHeader className="pb-3">
            <CardTitle className="flex items-center text-foreground">
              <XCircle className="h-5 w-5 mr-2 text-muted-foreground" />
              Absent Symptoms ({absentSymptoms.length})
            </CardTitle>
          </CardHeader>
          <CardContent>
            <div className="space-y-2">
              {absentSymptoms.length === 0 ? (
                <p className="text-muted-foreground text-center py-4">No absent symptoms recorded</p>
              ) : (
                absentSymptoms.map((symptom, index) => (
                  <div key={index} className="flex items-center p-2 border border-border rounded bg-muted/30">
                    <XCircle className="h-4 w-4 mr-2 text-muted-foreground" />
                    <span className="text-sm text-foreground">{symptom.name}</span>
                  </div>
                ))
              )}
            </div>
          </CardContent>
        </Card>
      </div>
    </div>
  );
};