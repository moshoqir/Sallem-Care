import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { CheckCircle, XCircle, Stethoscope } from "lucide-react";

export const SymptomsAnalysis = () => {
  const positiveSymptoms = [
    "Severe abdominal pain (RLQ)",
    "Nausea",
    "Loss of appetite",
    "Low-grade fever",
    "Guarding on palpation",
    "McBurney's point tenderness"
  ];

  const negativeSymptoms = [
    "No vomiting",
    "No diarrhea",
    "No dysuria",
    "No chest pain",
    "No shortness of breath"
  ];

  return (
    <Card>
      <CardHeader>
        <CardTitle className="flex items-center">
          <Stethoscope className="w-5 h-5 mr-2 text-primary" />
          Symptoms Analysis
        </CardTitle>
      </CardHeader>
      <CardContent>
        <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
          {/* Positive Symptoms */}
          <div>
            <h4 className="font-semibold text-foreground mb-3 flex items-center">
              <CheckCircle className="w-4 h-4 mr-2 text-success" />
              Present Symptoms ({positiveSymptoms.length})
            </h4>
            <div className="space-y-2">
              {positiveSymptoms.map((symptom, index) => (
                <div key={index} className="flex items-center space-x-2">
                  <CheckCircle className="w-4 h-4 text-success flex-shrink-0" />
                  <span className="text-sm text-foreground">{symptom}</span>
                </div>
              ))}
            </div>
          </div>

          {/* Negative Symptoms */}
          <div>
            <h4 className="font-semibold text-foreground mb-3 flex items-center">
              <XCircle className="w-4 h-4 mr-2 text-muted-foreground" />
              Absent Symptoms ({negativeSymptoms.length})
            </h4>
            <div className="space-y-2">
              {negativeSymptoms.map((symptom, index) => (
                <div key={index} className="flex items-center space-x-2">
                  <XCircle className="w-4 h-4 text-muted-foreground flex-shrink-0" />
                  <span className="text-sm text-muted-foreground">{symptom}</span>
                </div>
              ))}
            </div>
          </div>
        </div>

        <div className="mt-6 p-4 bg-secondary rounded-lg">
          <h5 className="font-medium text-secondary-foreground mb-2">Symptom Timeline</h5>
          <div className="flex items-center space-x-4 text-sm">
            <Badge variant="outline">0-2 hrs: Periumbilical pain</Badge>
            <Badge variant="outline">2-4 hrs: Pain migration to RLQ</Badge>
            <Badge variant="outline">4-6 hrs: Nausea onset</Badge>
          </div>
        </div>
      </CardContent>
    </Card>
  );
};