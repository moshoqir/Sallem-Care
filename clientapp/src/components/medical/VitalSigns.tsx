import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { Activity, Thermometer, Heart, Droplets, Wind } from "lucide-react";

export const VitalSigns = () => {
  const vitals = [
    {
      label: "Temperature",
      value: "38.2°C",
      normal: "36.5-37.5°C",
      status: "elevated",
      icon: Thermometer,
      unit: "(100.4°F)"
    },
    {
      label: "Heart Rate",
      value: "88 bpm",
      normal: "60-100 bpm",
      status: "normal",
      icon: Heart,
      unit: ""
    },
    {
      label: "Blood Pressure",
      value: "125/78",
      normal: "<120/80",
      status: "normal",
      icon: Droplets,
      unit: "mmHg"
    },
    {
      label: "Respiratory Rate",
      value: "18/min",
      normal: "12-20/min",
      status: "normal",
      icon: Wind,
      unit: ""
    },
    {
      label: "Oxygen Saturation",
      value: "98%",
      normal: ">95%",
      status: "normal",
      icon: Activity,
      unit: ""
    }
  ];

  const getStatusColor = (status: string) => {
    switch (status) {
      case "elevated": return "text-warning";
      case "high": return "text-destructive";
      case "low": return "text-destructive";
      case "normal": return "text-success";
      default: return "text-muted-foreground";
    }
  };

  const getStatusBadge = (status: string) => {
    switch (status) {
      case "elevated": return "bg-warning-light text-warning border-warning/20";
      case "high": return "bg-destructive-light text-destructive border-destructive/20";
      case "low": return "bg-destructive-light text-destructive border-destructive/20";
      case "normal": return "bg-success-light text-success border-success/20";
      default: return "bg-muted text-muted-foreground";
    }
  };

  return (
    <Card>
      <CardHeader>
        <CardTitle className="flex items-center">
          <Activity className="w-5 h-5 mr-2 text-primary" />
          Vital Signs
        </CardTitle>
      </CardHeader>
      <CardContent>
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
          {vitals.map((vital, index) => (
            <div key={index} className="border border-border rounded-lg p-4">
              <div className="flex items-center justify-between mb-2">
                <vital.icon className={`w-5 h-5 ${getStatusColor(vital.status)}`} />
                <Badge variant="outline" className={getStatusBadge(vital.status)}>
                  {vital.status.toUpperCase()}
                </Badge>
              </div>
              
              <h4 className="font-medium text-foreground text-sm">{vital.label}</h4>
              
              <div className="mt-2">
                <span className="text-2xl font-bold text-foreground">{vital.value}</span>
                {vital.unit && <span className="text-sm text-muted-foreground ml-1">{vital.unit}</span>}
              </div>
              
              <p className="text-xs text-muted-foreground mt-1">
                Normal: {vital.normal}
              </p>
            </div>
          ))}
        </div>
        
        <div className="mt-4 p-3 bg-muted rounded-lg">
          <p className="text-sm text-muted-foreground">
            <strong>Assessment:</strong> Mild hyperthermia consistent with inflammatory process. 
            Other vital signs within normal limits.
          </p>
        </div>
      </CardContent>
    </Card>
  );
};