import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Progress } from "@/components/ui/progress";
import { Badge } from "@/components/ui/badge";
import { Brain, Target, X } from "lucide-react";

interface Diagnosis {
  condition: string;
  probability: number;
  isPrimary?: boolean;
}

interface DiagnosticResultsProps {
  primaryDiagnosis: Diagnosis;
  differentialDiagnoses: Diagnosis[];
  excludedDiagnoses: string[];
  reasonsForExclusion: Record<string, string>;
}

export const DiagnosticResults = ({ 
  primaryDiagnosis, 
  differentialDiagnoses, 
  excludedDiagnoses, 
  reasonsForExclusion 
}: DiagnosticResultsProps) => {
  const getProbabilityColor = (probability: number) => {
    if (probability >= 80) return "bg-medical-success";
    if (probability >= 60) return "bg-medical-warning";
    return "bg-medical-danger";
  };

  return (
    <div className="grid gap-6 md:grid-cols-2">
      {/* Primary Diagnosis */}
      <Card className="shadow-card">
        <CardHeader className="pb-3">
          <CardTitle className="flex items-center text-foreground">
            <Target className="h-5 w-5 mr-2 text-primary" />
            Primary Diagnosis
          </CardTitle>
        </CardHeader>
        <CardContent>
          <div className="space-y-4">
            <div className="flex items-center justify-between">
              <h3 className="text-lg font-semibold text-foreground">{primaryDiagnosis.condition}</h3>
              <Badge className="bg-primary text-primary-foreground font-medium">
                {primaryDiagnosis.probability}% Confidence
              </Badge>
            </div>
            <Progress 
              value={primaryDiagnosis.probability} 
              className="h-3"
            />
            <div className="flex items-center text-sm text-muted-foreground">
              <Brain className="h-4 w-4 mr-1" />
              AI Analysis: High confidence based on symptom pattern matching
            </div>
          </div>
        </CardContent>
      </Card>

      {/* Differential Diagnoses */}
      <Card className="shadow-card">
        <CardHeader className="pb-3">
          <CardTitle className="flex items-center text-foreground">
            <Brain className="h-5 w-5 mr-2 text-primary" />
            Differential Diagnoses
          </CardTitle>
        </CardHeader>
        <CardContent>
          <div className="space-y-3">
            {differentialDiagnoses.map((diagnosis, index) => (
              <div key={index} className="space-y-2">
                <div className="flex items-center justify-between">
                  <span className="text-sm font-medium text-foreground">{diagnosis.condition}</span>
                  <span className="text-xs text-muted-foreground">{diagnosis.probability}%</span>
                </div>
                <Progress 
                  value={diagnosis.probability} 
                  className="h-2"
                />
              </div>
            ))}
          </div>
        </CardContent>
      </Card>

      {/* Excluded Diagnoses */}
      <Card className="shadow-card md:col-span-2">
        <CardHeader className="pb-3">
          <CardTitle className="flex items-center text-foreground">
            <X className="h-5 w-5 mr-2 text-muted-foreground" />
            Excluded Diagnoses
          </CardTitle>
        </CardHeader>
        <CardContent>
          <div className="grid gap-3 md:grid-cols-2">
            {excludedDiagnoses.map((diagnosis, index) => (
              <div key={index} className="p-3 border border-border rounded-lg bg-muted/30">
                <div className="flex items-start justify-between">
                  <span className="font-medium text-sm text-foreground">{diagnosis}</span>
                  <Badge variant="outline" className="text-xs border-muted-foreground/50">Excluded</Badge>
                </div>
                {reasonsForExclusion[diagnosis] && (
                  <p className="text-xs text-muted-foreground mt-1">
                    {reasonsForExclusion[diagnosis]}
                  </p>
                )}
              </div>
            ))}
          </div>
        </CardContent>
      </Card>
    </div>
  );
};