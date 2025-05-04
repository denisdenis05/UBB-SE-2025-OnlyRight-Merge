// <copyright file="ReviewViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using WinUIApp.Models;
    using WinUIApp.Services;

    /// <summary>
    /// ViewModel for managing reviews associated with ratings.
    /// </summary>
    public class ReviewViewModel
    {
        private const int DefaultUserId = 999;

        private readonly IReviewService reviewService;
        private ObservableCollection<Review> reviews;
        private Review? selectedReview;
        private string reviewContent = string.Empty;

        /// <summary>
        /// Event triggered when a property changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReviewViewModel"/> class.
        /// </summary>
        /// <param name="reviewService">The service used to manage reviews.</param>
        public ReviewViewModel(IReviewService reviewService)
        {
            this.reviewService = reviewService ?? throw new ArgumentNullException(nameof(reviewService));
            this.reviews = new ObservableCollection<Review>();
        }

        /// <summary>
        /// Event triggered when the window should be closed.
        /// </summary>
        public event EventHandler? RequestClose;

        /// <summary>
        /// Gets or sets the collection of reviews.
        /// </summary>
        public virtual ObservableCollection<Review> Reviews
        {
            get => this.reviews;
            set => this.SetProperty(ref this.reviews, value);
        }

        /// <summary>
        /// Gets or sets the selected review.
        /// </summary>
        public virtual Review? SelectedReview
        {
            get => this.selectedReview;
            set => this.SetProperty(ref this.selectedReview, value);
        }

        /// <summary>
        /// Gets or sets the content of the review.
        /// </summary>
        public virtual string ReviewContent
        {
            get => this.reviewContent;
            set => this.SetProperty(ref this.reviewContent, value);
        }

        /// <summary>
        /// Loads reviews for a specific rating based on its ID.
        /// </summary>
        /// <param name="ratingId">The ID of the rating whose reviews are to be loaded.</param>
        public virtual void LoadReviewsForRating(int ratingId)
        {
            var reviewsList = this.reviewService.GetReviewsByRating(ratingId);
            this.Reviews.Clear();
            foreach (var review in reviewsList)
            {
                this.Reviews.Add(review);
            }
        }

        /// <summary>
        /// Adds a new review for a specified rating.
        /// </summary>
        /// <param name="ratingId">The ID of the rating to which the review is to be added.</param>
        public virtual void AddReview(int ratingId)
        {
            if (string.IsNullOrWhiteSpace(this.ReviewContent))
            {
                return;
            }

            var newReview = new Review(
                drinkId: ratingId, // Assuming ratingId corresponds to drinkId
                reviewScore: 5.0f, // Default score, adjust as needed
                reviewerUserId: DefaultUserId,
                reviewTitle: "Default Title", // Replace with actual title if available
                reviewDescription: this.ReviewContent,
                reviewPostedDateTime: DateTime.Now
            );

            try
            {
                this.reviewService.AddReview(newReview);
            }
            catch (Exception exception)
            {
                Debug.WriteLine($"Error adding review: {exception.Message}");
                return;
            }

            this.LoadReviewsForRating(ratingId);
            this.ReviewContent = string.Empty;
            this.CloseWindow();
        }

        /// <summary>
        /// Clears the content of the current review.
        /// </summary>
        public virtual void ClearReviewContent()
        {
            this.ReviewContent = string.Empty;
        }

        private void CloseWindow()
        {
            this.RequestClose?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raises the PropertyChanged event for the specified property.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Sets the value of a property and raises the PropertyChanged event if the value has changed.
        /// </summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="storage">The backing field of the property.</param>
        /// <param name="value">The new value of the property.</param>
        /// <param name="propertyName">The name of the property being set (optional).</param>
        /// <returns>True if the value was changed, otherwise false.</returns>
        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
            {
                return false;
            }

            storage = value;
            this.OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Sets the value of a property, invokes an additional action, and raises the PropertyChanged event if the value has changed.
        /// </summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="storage">The backing field of the property.</param>
        /// <param name="value">The new value of the property.</param>
        /// <param name="onChanged">The action to invoke after the property is set.</param>
        /// <param name="propertyName">The name of the property being set (optional).</param>
        /// <returns>True if the value was changed, otherwise false.</returns>
        protected virtual bool SetProperty<T>(ref T storage, T value, Action onChanged, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
            {
                return false;
            }

            storage = value;
            onChanged?.Invoke();
            this.OnPropertyChanged(propertyName);
            return true;
        }
    }
}