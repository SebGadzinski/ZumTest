import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  results: any[] = [];
  isLoading: boolean = false;

  // Default sort settings
  sortBy: string = 'wins';
  sortDirection: string = 'desc';

  constructor(private http: HttpClient) { }

  startTournament() {
    this.isLoading = true;
    this.results = []; // Clear results before starting a new tournament

    const url = `/pokemon/tournament/statistics?sortBy=${this.sortBy}&sortDirection=${this.sortDirection}`;

    this.http.get<any[]>(url).subscribe(
      (data) => {
        this.results = data;
        this.isLoading = false;
      },
      (error) => {
        console.error('Error fetching tournament results:', error);
        this.isLoading = false;
      }
    );
  }
}
