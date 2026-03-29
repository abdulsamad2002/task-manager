import {
  Component,
  inject,
  input,
  output,
  computed,
} from '@angular/core';
import { WorkTask, TaskStatus } from '../../models/task.model';
import { TaskService } from '../../core/services/task.service';
import { AuthService } from '../../core/services/auth.service';
import { ToastService } from '../../core/services/toast.service';

@Component({
  selector: 'app-task-modal',
  standalone: true,
  template: `
    <div class="modal-overlay" (click)="onOverlayClick($event)">
      <div class="modal-box glass-card" id="modal-box">
        <button class="modal-close btn-icon" (click)="closed.emit()">✕</button>

        <div class="modal-header">
          <span class="badge badge-{{ priorityKey() }}">{{ task().priority }}</span>
          <h2 class="modal-title">{{ task().title }}</h2>
          <div class="modal-meta">
            <span class="badge badge-{{ statusKey() }}">{{ task().status }}</span>
            <span>📅 {{ formatDate(task().dueDate) }}{{ isOverdue() ? ' ⚠️ Overdue' : '' }}</span>
            @if (task().assignedToUserId) {
              <span>👤 User #{{ task().assignedToUserId }}</span>
            }
          </div>
        </div>

        <div class="modal-body">
          <p class="modal-desc">{{ task().description || 'No description provided.' }}</p>

          <div class="modal-status-section">
            <label class="form-label">Update Status</label>
            <div class="status-pills">
              @for (s of statuses; track s.value) {
                <button
                  class="status-pill"
                  [class.active]="task().status === s.value"
                  [disabled]="updating()"
                  (click)="changeStatus(s.value)">
                  {{ s.icon }} {{ s.label }}
                </button>
              }
            </div>
          </div>

          @if (auth.isAdmin) {
            <div class="modal-actions">
              <button class="btn btn-danger" [disabled]="deleting()" (click)="onDelete()">
                @if (deleting()) { <span class="spinner"></span> }
                🗑 Delete Task
              </button>
            </div>
          }
        </div>
      </div>
    </div>
  `,
  styles: [`
    .modal-overlay {
      position: fixed; inset: 0;
      background: rgba(0,0,0,.7); backdrop-filter: blur(8px);
      z-index: 500;
      display: flex; align-items: center; justify-content: center;
      padding: 24px;
      animation: fadeIn .2s ease;
    }
    @keyframes fadeIn { from { opacity: 0; } to { opacity: 1; } }
    .modal-box {
      width: 100%; max-width: 540px; padding: 36px; position: relative;
      animation: modalIn .3s var(--spring);
    }
    @keyframes modalIn {
      from { opacity: 0; transform: scale(.9) translateY(20px); }
      to   { opacity: 1; transform: scale(1) translateY(0); }
    }
    .modal-close {
      position: absolute; top: 16px; right: 16px;
      width: 32px; height: 32px;
      background: rgba(255,255,255,.07); border: 1px solid var(--glass-border);
      border-radius: 50%; color: var(--text-secondary);
      cursor: pointer; display: flex; align-items: center; justify-content: center;
      font-size: 14px; transition: background var(--transition), color var(--transition);
    }
    .modal-close:hover { background: rgba(255,255,255,.14); color: var(--text-primary); }
    .modal-header { margin-bottom: 24px; }
    .modal-title { font-size: 22px; font-weight: 700; line-height: 1.3; margin: 10px 0; }
    .modal-meta { display: flex; gap: 12px; flex-wrap: wrap; font-size: 13px; color: var(--text-secondary); align-items: center; }
    .modal-desc {
      font-size: 14px; color: var(--text-secondary); line-height: 1.7;
      padding: 16px; background: rgba(255,255,255,.03);
      border: 1px solid var(--glass-border); border-radius: var(--radius-md);
      margin-bottom: 24px; min-height: 60px;
    }
    .modal-status-section { margin-bottom: 24px; }
    .status-pills { display: flex; gap: 10px; margin-top: 10px; flex-wrap: wrap; }
    .status-pill {
      padding: 8px 18px; border-radius: 20px;
      border: 1px solid var(--glass-border);
      background: rgba(255,255,255,.04);
      color: var(--text-secondary);
      font-family: inherit; font-size: 13px; font-weight: 500;
      cursor: pointer; transition: all var(--transition);
    }
    .status-pill:hover:not(:disabled) { background: var(--accent-dim); border-color: var(--accent); color: var(--accent-light); }
    .status-pill.active { background: var(--accent-dim); border-color: var(--accent); color: var(--accent-light); font-weight: 600; }
    .modal-actions { display: flex; gap: 12px; }
    @media (max-width: 600px) { .modal-box { padding: 24px; } }
  `],
})
export class TaskModalComponent {
  task    = input.required<WorkTask>();
  closed  = output<void>();
  updated = output<void>();
  deleted = output<void>();

  taskSvc = inject(TaskService);
  auth    = inject(AuthService);
  toast   = inject(ToastService);

  updating = computed(() => false);  // replaced with local signal below
  deleting = computed(() => false);

  // local state
  private _updating = false;
  private _deleting = false;

  readonly statuses: { value: TaskStatus; label: string; icon: string }[] = [
    { value: 'Pending',     label: 'Pending',     icon: '🕐' },
    { value: 'In Progress', label: 'In Progress',  icon: '🔄' },
    { value: 'Completed',   label: 'Completed',   icon: '✅' },
  ];

  priorityKey = computed(() => this.task().priority || 'Low');
  statusKey   = computed(() => {
    const s = this.task().status;
    return s === 'Pending' ? 'pending' : s === 'In Progress' ? 'progress' : 'completed';
  });
  isOverdue   = computed(() =>
    !!this.task().dueDate &&
    new Date(this.task().dueDate) < new Date() &&
    this.task().status !== 'Completed'
  );

  onOverlayClick(e: MouseEvent): void {
    if ((e.target as HTMLElement).classList.contains('modal-overlay')) {
      this.closed.emit();
    }
  }

  changeStatus(newStatus: TaskStatus): void {
    if (this._updating || newStatus === this.task().status) return;
    this._updating = true;
    this.taskSvc.updateStatus(this.task().taskId, newStatus).subscribe({
      next: () => {
        this.toast.success(`Status → "${newStatus}"`);
        this.updated.emit();
        this._updating = false;
      },
      error: (err) => {
        this.toast.error(err?.error?.message || 'Failed to update status');
        this._updating = false;
      },
    });
  }

  onDelete(): void {
    if (!confirm(`Delete "${this.task().title}"? This cannot be undone.`)) return;
    this._deleting = true;
    this.taskSvc.deleteTask(this.task().taskId).subscribe({
      next: () => {
        this.toast.info('Task deleted.');
        this.deleted.emit();
        this._deleting = false;
      },
      error: (err) => {
        this.toast.error(err?.error?.message || 'Failed to delete task');
        this._deleting = false;
      },
    });
  }

  formatDate(d: string): string {
    if (!d) return '—';
    const dt = new Date(d);
    if (isNaN(dt.getTime())) return '—';
    return dt.toLocaleDateString('en-US', { year: 'numeric', month: 'short', day: 'numeric' });
  }
}
