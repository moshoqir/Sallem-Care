import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { AlertTriangle, Clock, ArrowRight } from "lucide-react";

export const SeverityIndicator = () => {
  return (
    <Card className="border-destructive/20">
      <CardHeader>
        <CardTitle className="flex items-center text-destructive">
          <AlertTriangle className="w-5 h-5 mr-2" />
          Severity Assessment
        </CardTitle>
      </CardHeader>
      <CardContent>
        <div className="space-y-4">
          {/* Severity Level */}
          <div className="text-center">
            <Badge className="bg-destructive text-destructive-foreground text-lg px-4 py-2">
              HIGH PRIORITY
            </Badge>
            <p className="text-sm text-muted-foreground mt-2">
              Requires immediate medical attention
            </p>
          </div>
          
          {/* Time Indicators */}
          <div className="bg-destructive-light rounded-lg p-4">
            <div className="flex items-center justify-between mb-2">
              <span className="text-sm font-medium text-destructive">Time to Treatment</span>
              <Clock className="w-4 h-4 text-destructive" />
            </div>
            <div className="text-2xl font-bold text-destructive">&lt; 4 hours</div>
            <p className="text-xs text-destructive mt-1">
              Risk of perforation increases significantly after 24 hours
            </p>
          </div>
          
          {/* Urgency Actions */}
          <div className="space-y-2">
            <h4 className="font-semibold text-foreground text-sm">Immediate Actions Required:</h4>
            <div className="space-y-2">
              <div className="flex items-center text-sm">
                <ArrowRight className="w-4 h-4 mr-2 text-destructive flex-shrink-0" />
                <span>Surgical consultation (STAT)</span>
              </div>
              <div className="flex items-center text-sm">
                <ArrowRight className="w-4 h-4 mr-2 text-destructive flex-shrink-0" />
                <span>NPO (Nothing by mouth)</span>
              </div>
              <div className="flex items-center text-sm">
                <ArrowRight className="w-4 h-4 mr-2 text-destructive flex-shrink-0" />
                <span>IV access and fluids</span>
              </div>
              <div className="flex items-center text-sm">
                <ArrowRight className="w-4 h-4 mr-2 text-destructive flex-shrink-0" />
                <span>Pain management</span>
              </div>
            </div>
          </div>
          
          <div className="mt-4 p-3 bg-warning-light rounded-lg">
            <p className="text-sm text-warning">
              <strong>Alert:</strong> Patient shows signs of acute surgical abdomen. 
              Do not delay treatment for additional testing.
            </p>
          </div>
        </div>
      </CardContent>
    </Card>
  );
};