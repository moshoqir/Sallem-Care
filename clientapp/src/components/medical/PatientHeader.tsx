import { Card } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { User, Calendar, Clock, Heart } from "lucide-react";

export const PatientHeader = () => {
  return (
    <Card className="p-6">
      <div className="flex items-start justify-between">
        <div className="flex items-center space-x-4">
          <div className="w-16 h-16 bg-primary-light rounded-full flex items-center justify-center">
            <User className="w-8 h-8 text-primary" />
          </div>
          <div>
            <h2 className="text-2xl font-bold text-foreground">Ahmed Hassan</h2>
            <p className="text-muted-foreground">Medical ID: #MR-2024-001234</p>
            <div className="flex items-center space-x-4 mt-2">
              <span className="text-sm text-muted-foreground">32 years old</span>
              <span className="text-sm text-muted-foreground">•</span>
              <span className="text-sm text-muted-foreground">Male</span>
              <span className="text-sm text-muted-foreground">•</span>
              <span className="text-sm text-muted-foreground">Married</span>
            </div>
          </div>
        </div>
        
        <div className="text-right space-y-2">
          <div className="flex items-center text-sm text-muted-foreground">
            <Calendar className="w-4 h-4 mr-2" />
            Exam Date: March 15, 2024
          </div>
          <div className="flex items-center text-sm text-muted-foreground">
            <Clock className="w-4 h-4 mr-2" />
            Time: 10:30 AM
          </div>
          <Badge variant="outline" className="bg-success-light text-success border-success/20">
            <Heart className="w-3 h-3 mr-1" />
            Active Patient
          </Badge>
        </div>
      </div>
      
      <div className="mt-4 pt-4 border-t border-border">
        <h3 className="text-lg font-semibold text-foreground mb-2">Chief Complaint</h3>
        <p className="text-foreground">Severe abdominal pain in the lower right quadrant</p>
        <div className="flex items-center space-x-4 mt-2 text-sm text-muted-foreground">
          <span>Duration: 6 hours</span>
          <span>•</span>
          <span>System: Digestive</span>
          <span>•</span>
          <Badge variant="secondary" className="text-xs">Acute Onset</Badge>
        </div>
      </div>
    </Card>
  );
};