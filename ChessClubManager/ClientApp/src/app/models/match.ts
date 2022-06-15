export class Match {
  id: string;    
  matchDate: Date;
  gamesPlayed: number;
  participants: Participant[] = [];
}

// helper classes for Match form.
export class Participant {
  id: string;
  matchId: string;
  memberId: string;
  matchResult: MatchResult;
}

export class FlatMatch {
  id: string;
  matchDate: Date;
  gamesPlayed: number;
  participant1Id: string;
  participant2Id: string;
  participant1Result: MatchResult;
  participant2Result: MatchResult;
}

export enum MatchResult {
  Draw = 0,
  Win = 1,
  Loss = 2
}
