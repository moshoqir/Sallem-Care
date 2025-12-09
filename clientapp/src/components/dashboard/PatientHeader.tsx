import { Card, CardContent } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { User, Calendar, Heart, AlertTriangle } from "lucide-react";

interface PatientInfo {
  fullName: string;
  medicalId: string;
  age: number;
  gender: string;
  dateOfExamination: string;
  maritalStatus?: string;
  specialConditions?: string[];
}

interface PatientHeaderProps {
  patient: PatientInfo;
  severity: "low" | "medium" | "high" | "critical";
}

export const PatientHeader = ({ patient, severity }: PatientHeaderProps) => {
  const getSeverityColor = (level: string) => {
    switch (level) {
      case "low": return "bg-medical-success text-medical-success-foreground";
      case "medium": return "bg-medical-warning text-medical-warning-foreground";
      case "high": return "bg-medical-danger text-medical-danger-foreground";
      case "critical": return "bg-severity-critical text-white";
      default: return "bg-muted text-muted-foreground";
    }
  };

  const getSeverityIcon = (level: string) => {
    switch (level) {
      case "critical":
      case "high":
        return <AlertTriangle className="h-4 w-4" />;
      default:
        return <Heart className="h-4 w-4" />;
    }
  };

  return (
    <Card className="shadow-card border-l-4 border-l-primary bg-gradient-medical text-primary-foreground">
      <CardContent className="p-6">
        <div className="flex items-start justify-between">
          <div className="flex items-start space-x-4">
            <div className="flex-shrink-0">
              <div className="w-16 h-16 bg-primary-foreground/20 rounded-full flex items-center justify-center">
                <User className="h-8 w-8 text-primary-foreground" />
              </div>
            </div>
            
            <div className="flex-1 min-w-0">
              <div className="flex items-center space-x-3 mb-2">
                <h1 className="text-2xl font-bold text-primary-foreground truncate">
                  {patient.fullName}
                </h1>
                <Badge className={`${getSeverityColor(severity)} font-medium`}>
                  {getSeverityIcon(severity)}
                  <span className="ml-1 capitalize">{severity} Risk</span>
                </Badge>
              </div>
              
              <div className="grid grid-cols-2 md:grid-cols-4 gap-4 text-sm text-primary-foreground/90">
                <div>
                  <p className="font-medium">Medical ID</p>
                  <p className="text-primary-foreground/70">{patient.medicalId}</p>
                </div>
                <div>
                  <p className="font-medium">Age & Gender</p>
                  <p className="text-primary-foreground/70">{patient.age} years, {patient.gender}</p>
                </div>
                <div className="flex items-center">
                  <Calendar className="h-4 w-4 mr-1" />
                  <div>
                    <p className="font-medium">Examination Date</p>
                    <p className="text-primary-foreground/70">{patient.dateOfExamination}</p>
                  </div>
                </div>
                {patient.maritalStatus && (
                  <div>
                    <p className="font-medium">Status</p>
                    <p className="text-primary-foreground/70">{patient.maritalStatus}</p>
                  </div>
                )}
              </div>
              
              {patient.specialConditions && patient.specialConditions.length > 0 && (
                <div className="mt-3">
                  <p className="font-medium text-sm mb-1">Special Conditions:</p>
                  <div className="flex flex-wrap gap-2">
                    {patient.specialConditions.map((condition, index) => (
                      <Badge key={index} variant="secondary" className="bg-primary-foreground/20 text-primary-foreground border-primary-foreground/30">
                        {condition}
                      </Badge>
                    ))}
                  </div>
                </div>
              )}
            </div>
          </div>
        </div>
      </CardContent>
    </Card>
  );
};