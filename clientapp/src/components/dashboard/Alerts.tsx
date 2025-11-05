import { Card, CardContent } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { AlertTriangle, Info, XCircle, Clock } from "lucide-react";

interface Alert {
  type: "warning" | "info" | "danger" | "urgent";
  message: string;
  priority: "low" | "medium" | "high" | "critical";
  timestamp?: string;
}

interface AlertsProps {
  alerts: Alert[];
}

export const Alerts = ({ alerts }: AlertsProps) => {
  if (alerts.length === 0) return null;

  const getAlertIcon = (type: string) => {
    switch (type) {
      case "danger":
      case "urgent":
        return <XCircle className="h-5 w-5" />;
      case "warning":
        return <AlertTriangle className="h-5 w-5" />;
      case "info":
        return <Info className="h-5 w-5" />;
      default:
        return <Clock className="h-5 w-5" />;
    }
  };

  const getAlertColor = (type: string, priority: string) => {
    if (priority === "critical") return "bg-severity-critical text-white border-severity-critical";
    
    switch (type) {
      case "danger":
      case "urgent":
        return "bg-medical-danger/10 text-medical-danger border-medical-danger";
      case "warning":
        return "bg-medical-warning/10 text-medical-warning border-medical-warning";
      case "info":
        return "bg-medical-info/10 text-medical-info border-medical-info";
      default:
        return "bg-muted/50 text-muted-foreground border-muted";
    }
  };

  const getPriorityBadge = (priority: string) => {
    switch (priority) {
      case "critical":
        return "bg-severity-critical text-white";
      case "high":
        return "bg-medical-danger text-medical-danger-foreground";
      case "medium":
        return "bg-medical-warning text-medical-warning-foreground";
      case "low":
        return "bg-medical-success text-medical-success-foreground";
      default:
        return "bg-muted text-muted-foreground";
    }
  };

  // Sort alerts by priority
  const sortedAlerts = [...alerts].sort((a, b) => {
    const priorityOrder = { critical: 4, high: 3, medium: 2, low: 1 };
    return priorityOrder[b.priority] - priorityOrder[a.priority];
  });

  return (
    <div className="space-y-3">
      {sortedAlerts.map((alert, index) => (
        <Card 
          key={index} 
          className={`border-l-4 ${getAlertColor(alert.type, alert.priority)} shadow-card`}
        >
          <CardContent className="p-4">
            <div className="flex items-start space-x-3">
              <div className={`${
                alert.priority === "critical" 
                  ? "text-white" 
                  : alert.type === "danger" || alert.type === "urgent"
                  ? "text-medical-danger"
                  : alert.type === "warning"
                  ? "text-medical-warning"
                  : "text-medical-info"
              }`}>
                {getAlertIcon(alert.type)}
              </div>
              
              <div className="flex-1 min-w-0">
                <div className="flex items-center justify-between mb-1">
                  <Badge className={`text-xs ${getPriorityBadge(alert.priority)}`}>
                    {alert.priority.toUpperCase()} PRIORITY
                  </Badge>
                  {alert.timestamp && (
                    <span className="text-xs text-muted-foreground">{alert.timestamp}</span>
                  )}
                </div>
                <p className={`text-sm font-medium ${
                  alert.priority === "critical" 
                    ? "text-white" 
                    : "text-foreground"
                }`}>
                  {alert.message}
                </p>
              </div>
            </div>
          </CardContent>
        </Card>
      ))}
    </div>
  );
};