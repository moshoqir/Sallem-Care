import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Textarea } from "@/components/ui/textarea";
import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";
import { FileText, Save, Edit, Clock, CheckCircle, XCircle } from "lucide-react";
import { useState } from "react";

export const PhysicianNotes = () => {
  const [notes, setNotes] = useState(
    "Patient presents with classic signs and symptoms consistent with acute appendicitis. Physical examination reveals McBurney's point tenderness, positive Rovsing's sign, and guarding. Laboratory findings support inflammatory process. Recommend immediate surgical consultation for appendectomy."
  );
  const [isEditing, setIsEditing] = useState(false);
  const [aiRecommendationStatus, setAiRecommendationStatus] = useState<"accepted" | "modified" | "rejected" | null>(null);

  const handleSave = () => {
    setIsEditing(false);
    // Here you would save to backend
  };

  const handleAiRecommendation = (status: "accepted" | "modified" | "rejected") => {
    setAiRecommendationStatus(status);
  };

  return (
    <Card>
      <CardHeader>
        <CardTitle className="flex items-center justify-between">
          <div className="flex items-center">
            <FileText className="w-5 h-5 mr-2 text-primary" />
            Physician Notes
          </div>
          <Button
            variant="outline"
            size="sm"
            onClick={() => setIsEditing(!isEditing)}
          >
            <Edit className="w-4 h-4 mr-2" />
            {isEditing ? "Cancel" : "Edit"}
          </Button>
        </CardTitle>
      </CardHeader>
      <CardContent className="space-y-4">
        {/* AI Recommendation Status */}
        <div className="border border-border rounded-lg p-4">
          <h4 className="font-semibold text-foreground mb-3">AI Recommendation Status</h4>
          
          <div className="flex items-center space-x-2 mb-3">
            <span className="text-sm text-muted-foreground">Current Status:</span>
            {aiRecommendationStatus === "accepted" && (
              <Badge className="bg-success text-success-foreground">
                <CheckCircle className="w-3 h-3 mr-1" />
                Accepted
              </Badge>
            )}
            {aiRecommendationStatus === "modified" && (
              <Badge className="bg-warning text-foreground">
                <Edit className="w-3 h-3 mr-1" />
                Modified
              </Badge>
            )}
            {aiRecommendationStatus === "rejected" && (
              <Badge className="bg-destructive text-destructive-foreground">
                <XCircle className="w-3 h-3 mr-1" />
                Rejected
              </Badge>
            )}
            {aiRecommendationStatus === null && (
              <Badge variant="outline">Pending Review</Badge>
            )}
          </div>
          
          <div className="flex space-x-2">
            <Button
              size="sm"
              variant={aiRecommendationStatus === "accepted" ? "default" : "outline"}
              onClick={() => handleAiRecommendation("accepted")}
            >
              <CheckCircle className="w-4 h-4 mr-1" />
              Accept
            </Button>
            <Button
              size="sm"
              variant={aiRecommendationStatus === "modified" ? "default" : "outline"}
              onClick={() => handleAiRecommendation("modified")}
            >
              <Edit className="w-4 h-4 mr-1" />
              Modify
            </Button>
            <Button
              size="sm"
              variant={aiRecommendationStatus === "rejected" ? "destructive" : "outline"}
              onClick={() => handleAiRecommendation("rejected")}
            >
              <XCircle className="w-4 h-4 mr-1" />
              Reject
            </Button>
          </div>
        </div>

        {/* Physician Notes */}
        <div>
          <h4 className="font-semibold text-foreground mb-3">Clinical Notes</h4>
          {isEditing ? (
            <div className="space-y-3">
              <Textarea
                value={notes}
                onChange={(e) => setNotes(e.target.value)}
                className="min-h-32"
                placeholder="Enter your clinical notes here..."
              />
              <div className="flex justify-end space-x-2">
                <Button variant="outline" onClick={() => setIsEditing(false)}>
                  Cancel
                </Button>
                <Button onClick={handleSave}>
                  <Save className="w-4 h-4 mr-2" />
                  Save Notes
                </Button>
              </div>
            </div>
          ) : (
            <div className="bg-muted rounded-lg p-4">
              <p className="text-foreground whitespace-pre-wrap">{notes}</p>
            </div>
          )}
        </div>

        {/* Previous Visit History */}
        <div className="border-t border-border pt-4">
          <h4 className="font-semibold text-foreground mb-3">Visit History</h4>
          <div className="space-y-3">
            <div className="bg-secondary rounded-lg p-3">
              <div className="flex items-center justify-between mb-2">
                <span className="font-medium text-secondary-foreground">Previous Visit</span>
                <div className="flex items-center text-sm text-muted-foreground">
                  <Clock className="w-4 h-4 mr-1" />
                  March 10, 2024
                </div>
              </div>
              <p className="text-sm text-secondary-foreground">
                Patient complained of intermittent abdominal discomfort. 
                Diagnosed with gastritis. Prescribed PPI therapy. Symptoms resolved.
              </p>
            </div>
            
            <div className="bg-secondary rounded-lg p-3">
              <div className="flex items-center justify-between mb-2">
                <span className="font-medium text-secondary-foreground">Annual Physical</span>
                <div className="flex items-center text-sm text-muted-foreground">
                  <Clock className="w-4 h-4 mr-1" />
                  January 15, 2024
                </div>
              </div>
              <p className="text-sm text-secondary-foreground">
                Routine annual examination. All parameters within normal limits. 
                No significant medical history. Recommended preventive care measures.
              </p>
            </div>
          </div>
        </div>

        {/* Timestamp */}
        <div className="text-xs text-muted-foreground flex items-center justify-end">
          <Clock className="w-3 h-3 mr-1" />
          Last updated: March 15, 2024 at 10:45 AM by Dr. Sarah Mitchell
        </div>
      </CardContent>
    </Card>
  );
};