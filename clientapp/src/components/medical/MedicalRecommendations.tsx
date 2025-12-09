import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { Pill, Stethoscope, TestTube, UserCheck, AlertCircle } from "lucide-react";

export const MedicalRecommendations = () => {
  const recommendations = [
    {
      category: "Immediate Treatment",
      icon: Stethoscope,
      items: [
        "Emergency appendectomy (laparoscopic preferred)",
        "Pre-operative antibiotics (Cefoxitin 2g IV)",
        "IV fluid resuscitation (Normal Saline 500ml/hr)",
        "Pain management (Morphine 2-4mg IV q4h PRN)"
      ],
      priority: "urgent"
    },
    {
      category: "Diagnostic Tests",
      icon: TestTube,
      items: [
        "CT abdomen/pelvis with IV contrast (STAT)",
        "Complete Blood Count with differential",
        "Comprehensive Metabolic Panel",
        "Urinalysis to rule out UTI"
      ],
      priority: "immediate"
    },
    {
      category: "Consultations",
      icon: UserCheck,
      items: [
        "General Surgery (STAT)",
        "Anesthesiology (for OR prep)",
        "Consider GI if atypical presentation"
      ],
      priority: "urgent"
    }
  ];

  const getPriorityColor = (priority: string) => {
    switch (priority) {
      case "urgent": return "bg-destructive text-destructive-foreground";
      case "immediate": return "bg-warning text-foreground";
      case "routine": return "bg-success text-success-foreground";
      default: return "bg-muted text-muted-foreground";
    }
  };

  return (
    <Card>
      <CardHeader>
        <CardTitle className="flex items-center">
          <Pill className="w-5 h-5 mr-2 text-primary" />
          Medical Recommendations
        </CardTitle>
      </CardHeader>
      <CardContent className="space-y-4">
        {recommendations.map((rec, index) => (
          <div key={index} className="border border-border rounded-lg p-4">
            <div className="flex items-center justify-between mb-3">
              <div className="flex items-center">
                <rec.icon className="w-5 h-5 mr-2 text-primary" />
                <h4 className="font-semibold text-foreground">{rec.category}</h4>
              </div>
              <Badge className={getPriorityColor(rec.priority)}>
                {rec.priority.toUpperCase()}
              </Badge>
            </div>
            
            <ul className="space-y-2">
              {rec.items.map((item, itemIndex) => (
                <li key={itemIndex} className="flex items-start">
                  <div className="w-2 h-2 bg-primary rounded-full mt-2 mr-3 flex-shrink-0"></div>
                  <span className="text-sm text-foreground">{item}</span>
                </li>
              ))}
            </ul>
          </div>
        ))}
        
        {/* Drug Interaction Alerts */}
        <div className="bg-warning-light border border-warning/20 rounded-lg p-4">
          <div className="flex items-start">
            <AlertCircle className="w-5 h-5 mr-2 mt-0.5 text-warning flex-shrink-0" />
            <div>
              <h5 className="font-medium text-warning">Drug Interaction Alert</h5>
              <p className="text-sm text-warning mt-1">
                No known drug allergies or interactions with current medications. 
                Monitor for respiratory depression with opioid pain management.
              </p>
            </div>
          </div>
        </div>
        
        {/* Follow-up Instructions */}
        <div className="bg-primary-light rounded-lg p-4">
          <h5 className="font-medium text-primary mb-2">Post-Operative Follow-up</h5>
          <ul className="text-sm text-primary space-y-1">
            <li>• 2-week post-op visit with surgeon</li>
            <li>• Return to ED if fever, increasing pain, or wound complications</li>
            <li>• Activity restrictions: No heavy lifting for 2 weeks</li>
            <li>• Diet advancement as tolerated post-operatively</li>
          </ul>
        </div>
      </CardContent>
    </Card>
  );
};