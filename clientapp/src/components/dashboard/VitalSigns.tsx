import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { Thermometer, Activity, Heart, Wind, Droplets } from "lucide-react";

interface VitalSign {
  label: string;
  value: string;
  unit: string;
  normalRange: string;
  status: "normal" | "warning" | "danger";
  icon: React.ReactNode;
}

interface VitalSignsProps {
  vitals: {
    temperature?: { value: number; unit: string };
    pulse?: { value: number; unit: string };
    bloodPressure?: { systolic: number; diastolic: number };
    respiratoryRate?: { value: number; unit: string };
    oxygenSaturation?: { value: number; unit: string };
  };
}

export const VitalSigns = ({ vitals }: VitalSignsProps) => {
  const getVitalStatus = (type: string, value: any): "normal" | "warning" | "danger" => {
    switch (type) {
      case "temperature":
        const temp = parseFloat(value);
        if (temp >= 100.4 || temp <= 95) return "danger";
        if (temp >= 99.5 || temp <= 96) return "warning";
        return "normal";
      case "pulse":
        const pulse = parseInt(value);
        if (pulse > 100 || pulse < 60) return "warning";
        if (pulse > 120 || pulse < 50) return "danger";
        return "normal";
      case "bloodPressure":
        const { systolic, diastolic } = value;
        if (systolic >= 140 || diastolic >= 90) return "danger";
        if (systolic >= 130 || diastolic >= 80) return "warning";
        return "normal";
      case "respiratoryRate":
        const rate = parseInt(value);
        if (rate > 20 || rate < 12) return "warning";
        if (rate > 24 || rate < 10) return "danger";
        return "normal";
      case "oxygenSaturation":
        const o2 = parseInt(value);
        if (o2 < 95) return "danger";
        if (o2 < 98) return "warning";
        return "normal";
      default:
        return "normal";
    }
  };

  const getStatusColor = (status: string) => {
    switch (status) {
      case "normal": return "bg-medical-success text-medical-success-foreground";
      case "warning": return "bg-medical-warning text-medical-warning-foreground";
      case "danger": return "bg-medical-danger text-medical-danger-foreground";
      default: return "bg-muted text-muted-foreground";
    }
  };

  const vitalSigns: VitalSign[] = [];

  if (vitals.temperature) {
    vitalSigns.push({
      label: "Temperature",
      value: vitals.temperature.value.toString(),
      unit: vitals.temperature.unit,
      normalRange: "97.0-99.0°F",
      status: getVitalStatus("temperature", vitals.temperature.value),
      icon: <Thermometer className="h-5 w-5" />
    });
  }

  if (vitals.pulse) {
    vitalSigns.push({
      label: "Pulse Rate",
      value: vitals.pulse.value.toString(),
      unit: vitals.pulse.unit,
      normalRange: "60-100 bpm",
      status: getVitalStatus("pulse", vitals.pulse.value),
      icon: <Heart className="h-5 w-5" />
    });
  }

  if (vitals.bloodPressure) {
    vitalSigns.push({
      label: "Blood Pressure",
      value: `${vitals.bloodPressure.systolic}/${vitals.bloodPressure.diastolic}`,
      unit: "mmHg",
      normalRange: "<120/80 mmHg",
      status: getVitalStatus("bloodPressure", vitals.bloodPressure),
      icon: <Activity className="h-5 w-5" />
    });
  }

  if (vitals.respiratoryRate) {
    vitalSigns.push({
      label: "Respiratory Rate",
      value: vitals.respiratoryRate.value.toString(),
      unit: vitals.respiratoryRate.unit,
      normalRange: "12-20 /min",
      status: getVitalStatus("respiratoryRate", vitals.respiratoryRate.value),
      icon: <Wind className="h-5 w-5" />
    });
  }

  if (vitals.oxygenSaturation) {
    vitalSigns.push({
      label: "Oxygen Saturation",
      value: vitals.oxygenSaturation.value.toString(),
      unit: vitals.oxygenSaturation.unit,
      normalRange: "95-100%",
      status: getVitalStatus("oxygenSaturation", vitals.oxygenSaturation.value),
      icon: <Droplets className="h-5 w-5" />
    });
  }

  if (vitalSigns.length === 0) {
    return (
      <Card className="shadow-card">
        <CardHeader>
          <CardTitle className="text-foreground">Vital Signs</CardTitle>
        </CardHeader>
        <CardContent>
          <p className="text-muted-foreground text-center py-4">No vital signs recorded</p>
        </CardContent>
      </Card>
    );
  }

  return (
    <Card className="shadow-card">
      <CardHeader>
        <CardTitle className="text-foreground">Vital Signs</CardTitle>
      </CardHeader>
      <CardContent>
        <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-3">
          {vitalSigns.map((vital, index) => (
            <div key={index} className="p-4 border border-border rounded-lg bg-muted/30">
              <div className="flex items-center justify-between mb-2">
                <div className="flex items-center text-foreground">
                  {vital.icon}
                  <span className="ml-2 font-medium text-sm">{vital.label}</span>
                </div>
                <Badge className={`text-xs ${getStatusColor(vital.status)}`}>
                  {vital.status}
                </Badge>
              </div>
              <div className="space-y-1">
                <div className="text-lg font-bold text-foreground">
                  {vital.value} <span className="text-sm font-normal text-muted-foreground">{vital.unit}</span>
                </div>
                <div className="text-xs text-muted-foreground">
                  Normal: {vital.normalRange}
                </div>
              </div>
            </div>
          ))}
        </div>
      </CardContent>
    </Card>
  );
};