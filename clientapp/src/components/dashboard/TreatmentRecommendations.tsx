import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";
import { Pill, Clock, AlertTriangle, UserCheck, Calendar } from "lucide-react";

interface Medication {
  name: string;
  dosage: string;
  frequency: string;
  duration: string;
  instructions: string;
  warnings?: string[];
}

interface Recommendation {
  type: "medication" | "intervention" | "followup" | "referral";
  title: string;
  description: string;
  priority: "low" | "medium" | "high";
  timeframe?: string;
}

export const TreatmentRecommendations = () => {
  const medications: Medication[] = [
    {
      name: "Amoxicillin",
      dosage: "500mg",
      frequency: "3 times daily",
      duration: "10 days",
      instructions: "Take with food to reduce stomach upset",
      warnings: ["Safe during pregnancy", "Complete full course"]
    },
    {
      name: "Acetaminophen",
      dosage: "650mg",
      frequency: "Every 6 hours as needed",
      duration: "As needed",
      instructions: "For pain and fever relief",
      warnings: ["Do not exceed 3000mg daily", "Safe during pregnancy"]
    }
  ];

  const recommendations: Recommendation[] = [
    {
      type: "intervention",
      title: "Supportive Care",
      description: "Increase fluid intake, warm salt water gargles, throat lozenges",
      priority: "medium",
      timeframe: "Immediate"
    },
    {
      type: "followup",
      title: "Follow-up Appointment",
      description: "Return in 3-5 days if symptoms persist or worsen",
      priority: "medium",
      timeframe: "3-5 days"
    },
    {
      type: "referral",
      title: "ENT Consultation",
      description: "Consider if no improvement after antibiotic course",
      priority: "low",
      timeframe: "If needed"
    }
  ];

  const getPriorityColor = (priority: string) => {
    switch (priority) {
      case "high": return "bg-medical-danger text-medical-danger-foreground";
      case "medium": return "bg-medical-warning text-medical-warning-foreground";
      case "low": return "bg-medical-success text-medical-success-foreground";
      default: return "bg-muted text-muted-foreground";
    }
  };

  const getRecommendationIcon = (type: string) => {
    switch (type) {
      case "medication": return <Pill className="h-4 w-4" />;
      case "intervention": return <UserCheck className="h-4 w-4" />;
      case "followup": return <Calendar className="h-4 w-4" />;
      case "referral": return <Clock className="h-4 w-4" />;
      default: return <UserCheck className="h-4 w-4" />;
    }
  };

  return (
    <div className="grid gap-6 lg:grid-cols-2">
      {/* Medications */}
      <Card className="shadow-card">
        <CardHeader>
          <CardTitle className="flex items-center text-foreground">
            <Pill className="h-5 w-5 mr-2 text-primary" />
            Prescribed Medications
          </CardTitle>
        </CardHeader>
        <CardContent>
          <div className="space-y-4">
            {medications.map((med, index) => (
              <div key={index} className="p-4 border border-border rounded-lg bg-muted/30">
                <div className="flex items-start justify-between mb-2">
                  <h3 className="font-semibold text-foreground">{med.name}</h3>
                  <Badge className="bg-primary text-primary-foreground text-xs">
                    {med.duration}
                  </Badge>
                </div>
                
                <div className="space-y-2 text-sm">
                  <div className="grid grid-cols-2 gap-2">
                    <span className="text-muted-foreground">Dosage:</span>
                    <span className="text-foreground font-medium">{med.dosage}</span>
                    <span className="text-muted-foreground">Frequency:</span>
                    <span className="text-foreground font-medium">{med.frequency}</span>
                  </div>
                  
                  <div className="pt-2">
                    <p className="text-foreground"><strong>Instructions:</strong> {med.instructions}</p>
                  </div>
                  
                  {med.warnings && med.warnings.length > 0 && (
                    <div className="pt-2">
                      <div className="flex items-start">
                        <AlertTriangle className="h-4 w-4 text-medical-warning mr-1 mt-0.5 flex-shrink-0" />
                        <div>
                          <p className="text-xs font-medium text-medical-warning">Important Notes:</p>
                          <ul className="text-xs text-muted-foreground mt-1">
                            {med.warnings.map((warning, idx) => (
                              <li key={idx}>• {warning}</li>
                            ))}
                          </ul>
                        </div>
                      </div>
                    </div>
                  )}
                </div>
              </div>
            ))}
          </div>
        </CardContent>
      </Card>

      {/* Other Recommendations */}
      <Card className="shadow-card">
        <CardHeader>
          <CardTitle className="flex items-center text-foreground">
            <UserCheck className="h-5 w-5 mr-2 text-primary" />
            Additional Recommendations
          </CardTitle>
        </CardHeader>
        <CardContent>
          <div className="space-y-3">
            {recommendations.map((rec, index) => (
              <div key={index} className="p-3 border border-border rounded-lg bg-muted/30">
                <div className="flex items-start justify-between mb-2">
                  <div className="flex items-center">
                    {getRecommendationIcon(rec.type)}
                    <h4 className="font-medium text-foreground ml-2">{rec.title}</h4>
                  </div>
                  <div className="flex items-center space-x-2">
                    {rec.timeframe && (
                      <Badge variant="outline" className="text-xs">
                        {rec.timeframe}
                      </Badge>
                    )}
                    <Badge className={`text-xs ${getPriorityColor(rec.priority)}`}>
                      {rec.priority}
                    </Badge>
                  </div>
                </div>
                <p className="text-sm text-muted-foreground">{rec.description}</p>
              </div>
            ))}
          </div>
          
          <div className="mt-4 pt-4 border-t border-border">
            <Button className="w-full bg-primary text-primary-foreground hover:bg-primary/90">
              Generate Treatment Plan
            </Button>
          </div>
        </CardContent>
      </Card>
    </div>
  );
};
