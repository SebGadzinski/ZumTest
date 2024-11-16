import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})

export class AppComponent {
  results: any[] = [];
  isLoading: boolean = false;

  constructor(private http: HttpClient) { }

  startTournament() {
    this.isLoading = true;
    this.results = []; // Clear results before starting a new tournament

    this.http.get<any[]>('/pokemon/tournament/statistics?sortBy=wins&sortDirection=desc')
      .subscribe(
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
