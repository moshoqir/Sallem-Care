import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { Progress } from "@/components/ui/progress";
import { Brain, TrendingUp, AlertTriangle } from "lucide-react";

export const DiagnosticResults = () => {
  const diagnoses = [
    { 
      condition: "Acute Appendicitis", 
      probability: 92, 
      severity: "high",
      reasoning: "Classic presentation with McBurney's point tenderness, elevated WBC count, and symptom progression"
    },
    { 
      condition: "Gastroenteritis", 
      probability: 15, 
      severity: "low",
      reasoning: "Less likely due to localized pain pattern and absence of diarrhea"
    },
    { 
      condition: "Ovarian Cyst", 
      probability: 8, 
      severity: "low",
      reasoning: "Ruled out - patient is male"
    }
  ];

  const getSeverityColor = (severity: string) => {
    switch (severity) {
      case "high": return "bg-destructive text-destructive-foreground";
      case "medium": return "bg-warning text-foreground";
      case "low": return "bg-success text-success-foreground";
      default: return "bg-muted text-muted-foreground";
    }
  };

  return (
    <Card>
      <CardHeader>
        <CardTitle className="flex items-center">
          <Brain className="w-5 h-5 mr-2 text-primary" />
          AI Diagnostic Analysis
        </CardTitle>
      </CardHeader>
      <CardContent className="space-y-4">
        {diagnoses.map((diagnosis, index) => (
          <div key={index} className="border border-border rounded-lg p-4">
            <div className="flex items-center justify-between mb-2">
              <h4 className="font-semibold text-foreground">{diagnosis.condition}</h4>
              <div className="flex items-center space-x-2">
                <Badge className={getSeverityColor(diagnosis.severity)}>
                  {diagnosis.severity.toUpperCase()}
                </Badge>
                <span className="text-lg font-bold text-primary">{diagnosis.probability}%</span>
              </div>
            </div>
            
            <Progress value={diagnosis.probability} className="mb-3" />
            
            <div className="bg-muted rounded-md p-3">
              <div className="flex items-start">
                <TrendingUp className="w-4 h-4 mr-2 mt-0.5 text-muted-foreground flex-shrink-0" />
                <p className="text-sm text-muted-foreground">{diagnosis.reasoning}</p>
              </div>
            </div>
          </div>
        ))}
        
        <div className="mt-4 p-3 bg-primary-light rounded-lg">
          <div className="flex items-start">
            <AlertTriangle className="w-5 h-5 mr-2 mt-0.5 text-primary flex-shrink-0" />
            <div>
              <h5 className="font-medium text-primary">Clinical Decision Support</h5>
              <p className="text-sm text-primary mt-1">
                High probability of acute appendicitis requires immediate surgical consultation. 
                CT scan recommended for confirmation before surgical intervention.
              </p>
            </div>
          </div>
        </div>
      </CardContent>
    </Card>
  );
};